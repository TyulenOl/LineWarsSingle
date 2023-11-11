using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace LineWars.Model
{
    public partial class EnemyAI : BasePlayer
    {
        [SerializeField] private EnemyDifficulty difficulty;
        [SerializeField] private float actionCooldown;
        [SerializeField] private EnemyAIPersonality personality;
        [SerializeField] private GameEvaluator gameEvaluator;
        [SerializeField] private int depth;
        [SerializeField] private float commandPause;
        [SerializeField] private float firstCommandPause;

        private EnemyAIBuySelector buySelector;

        protected override void Awake()
        {
            base.Awake();
            buySelector = new EnemyAIBuySelector();
        }

        #region Turns

        public override void ExecuteBuy()
        {
            StartCoroutine(BuyCoroutine());
            IEnumerator BuyCoroutine()
            {
                if (buySelector.TryGetPreset(this, out var preset))
                    BuyPreset(preset);
                yield return null;
                ExecuteTurn(PhaseType.Idle);
            }
        }
        public override void ExecuteArtillery() => ExecuteAITurn(PhaseType.Artillery);
        public override void ExecuteFight() => ExecuteAITurn(PhaseType.Fight);
        public override void ExecuteScout() => ExecuteAITurn(PhaseType.Scout);

        public override void ExecuteReplenish()
        {
            base.ExecuteReplenish();
            StartCoroutine(ReplenishCoroutine());
            IEnumerator ReplenishCoroutine()
            {
                yield return null;
                ExecuteTurn(PhaseType.Idle);
            }
        }

        public async void ExecuteAITurn(PhaseType phase)
        {
            var gameProjection = 
                GameProjectionCreator.FromMono(SingleGame.Instance.AllPlayers.Values, MonoGraph.Instance, PhaseManager.Instance);
            var possibleCommands = CommandBlueprintCollector.CollectAllCommands(gameProjection);
            var tasksList = new List<Task<(int, List<ICommandBlueprint>)>>();
            foreach ( var command in possibleCommands )
            {
                var commandChain = new List<ICommandBlueprint>();
                tasksList.Add(ExploreOutcomes(gameProjection, command, depth, -1, commandChain, true));
            }

            var commandEvalList = await Task.WhenAll(tasksList.ToArray());

            StartCoroutine(TurnCoroutine());

            IEnumerator TurnCoroutine()
            {
                yield return new WaitForSeconds(firstCommandPause);
                var bestBlueprint = commandEvalList.MaxItem((i1, i2) => i1.Item1.CompareTo(i2.Item1));
                foreach (var blueprint in bestBlueprint.Item2)
                {
                    var command = blueprint.GenerateMonoCommand(gameProjection);
                    UnitsController.ExecuteCommand(command);
                    yield return new WaitForSeconds(commandPause);
                }
                yield return null;
                ExecuteTurn(PhaseType.Idle);
            }         
        }


        private Task<(int, List<ICommandBlueprint>)> ExploreOutcomes(GameProjection gameProjection, ICommandBlueprint blueprint, int depth,
            int currentExecutorId, List<ICommandBlueprint> firstCommandChain, bool isSavingCommands)
        {
            var task = new Task<(int, List<ICommandBlueprint>)>(
                () => MinMax(gameProjection, blueprint, depth, currentExecutorId, firstCommandChain, isSavingCommands));
            task.Start();
            return task;
        }

        private (int, List<ICommandBlueprint>) MinMax(GameProjection gameProjection, ICommandBlueprint blueprint, int depth, 
            int currentExecutorId, List<ICommandBlueprint> firstCommandChain, bool isSavingCommands)
        {
            if(currentExecutorId != -1 && blueprint.ExecutorId != currentExecutorId)
                throw new ArgumentException();

            currentExecutorId = blueprint.ExecutorId;
            var newGame = GameProjectionCreator.FromProjection(gameProjection);
            var thisCommand = blueprint.GenerateCommand(newGame);
            thisCommand.Execute();

            if(isSavingCommands)
            {
                var newCommandChain = new List<ICommandBlueprint>(firstCommandChain);
                newCommandChain.Add(blueprint);
                firstCommandChain = newCommandChain;
            }

            var thisPlayerProjection = newGame.OriginalToProjectionPlayers[this];
            if (newGame.UnitsIndexList.Count == 0)
                return (gameEvaluator.Evaluate(newGame, thisPlayerProjection), firstCommandChain); 
            
            if (IsTurnOver(newGame, currentExecutorId))
            {
                depth--;
                currentExecutorId = -1;
                isSavingCommands = false;
                if (!newGame.IsUnitPhaseAvailable())
                    newGame.CycleTurn();
                else
                    newGame.CyclePlayers();
            }
            if (depth == 0 || newGame.CurrentPhase == PhaseType.Buy)
            {
                return (gameEvaluator.Evaluate(newGame, thisPlayerProjection), firstCommandChain);
            }

            var possibleCommands = CommandBlueprintCollector.CollectAllCommands(newGame)
            .Where(newBlueprint => currentExecutorId == -1 || newBlueprint.ExecutorId == currentExecutorId)
            .Select(newBlueprint => MinMax(newGame, newBlueprint, depth, currentExecutorId, firstCommandChain, isSavingCommands));

            if (thisPlayerProjection != newGame.CurrentPlayer)
            {
                var minChain = possibleCommands.MinItem((i1, i2) => i1.Item1.CompareTo(i2.Item1));
                return minChain;
            }
            else
            {
                var maxChain = possibleCommands.MaxItem((i1, i2) => i1.Item1.CompareTo(i2.Item1));
                return maxChain;
            }
        }

        private bool IsTurnOver(GameProjection game, int currentExecutorId)
        {
            if(game.CurrentPhase != PhaseType.Buy && currentExecutorId != -1)
            {
                if (!game.UnitsIndexList.ContainsKey(currentExecutorId)) 
                    return true;
                var currentExecutor = game.UnitsIndexList[currentExecutorId];
                return currentExecutor.CurrentActionPoints <= 0;
            }
            return true;
        }
        #endregion

        #region Check Turns
        public override bool CanExecuteBuy() => true;
        public override bool CanExecuteArtillery() => CanExecutePhase(PhaseType.Artillery);
        public override bool CanExecuteFight() => CanExecutePhase(PhaseType.Fight);
        public override bool CanExecuteScout() => CanExecutePhase(PhaseType.Scout);
        public override bool CanExecuteReplenish() => true;

        private bool CanExecutePhase(PhaseType phase)
        {
            var executors = PhaseExecutorsData.PhaseToUnits[phase];
            foreach (var owned in OwnedObjects)
            {
                if (!(owned is Unit unit)) continue;
                if (executors.Contains(unit.Type) && unit.CurrentActionPoints > 0)
                    return true;
            }

            return false;
        }
        #endregion
    }
}

