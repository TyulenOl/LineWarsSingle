using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private IExecutor currentExecutor;
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
                    SpawnPreset(preset);
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

        public void ExecuteAITurn(PhaseType phase)
        {
            var gameProjection = 
                GameProjection.GetProjectionFromMono(SingleGame.Instance.AllPlayers.Values, MonoGraph.Instance, PhaseManager.Instance);
            
            var possibleCommands = CommandBlueprintCollector.CollectAllCommands(gameProjection);
            var commandEvalList = new List<(ICommandBlueprint, int)>(); 

            foreach (var blueprint in possibleCommands)
            {
                var newGame = GetProjectionFromCommand(gameProjection, blueprint);
                var eval = MinMax(newGame, depth - 1);
                commandEvalList.Add((blueprint, eval));
            }

            Debug.Log(commandEvalList.Count);
            foreach(var command in commandEvalList)
            {
                Debug.Log(command.ToString());
            }
            StartCoroutine(TurnCoroutine());
            IEnumerator TurnCoroutine()
            {
                var bestBlueprint = commandEvalList.Aggregate((i1, i2) => i1.Item2 > i2.Item2 ? i1 : i2);
                var command = bestBlueprint.Item1.GenerateMonoCommand(gameProjection);
                Debug.Log(command.ToString());
                command.Execute();
                yield return null;
                ExecuteTurn(PhaseType.Idle);
            }
            
        }

        private int MinMax(GameProjection gameProjection, int depth)
        {
            var thisPlayerPojection = gameProjection.OriginalToProjectionPlayers[this];
            if (depth == 0 || gameProjection.CurrentPhase == PhaseType.Buy)
                return gameEvaluator.Evaluate(gameProjection, thisPlayerPojection);

            var possibleCommands = CommandBlueprintCollector.CollectAllCommands(gameProjection);
            if (thisPlayerPojection == gameProjection.CurrentPlayer)
            {
                var maxEval = int.MinValue;
                foreach (var blueprint in possibleCommands)
                {
                    var newGame = GetProjectionFromCommand(gameProjection, blueprint);
                    var eval = MinMax(newGame, depth - 1);
                    maxEval = Mathf.Max(maxEval, eval);
                }
                return maxEval;
            }
            var minEval = int.MaxValue;
            foreach (var blueprint in possibleCommands)
            {
                var newGame = GetProjectionFromCommand(gameProjection, blueprint);
                var eval = MinMax(newGame, depth - 1);
                minEval = Mathf.Min(minEval, eval);
            }

            return minEval;
        }

        private GameProjection GetProjectionFromCommand(GameProjection game, ICommandBlueprint blueprint)
        {
            var newGame = GameProjection.GetCopy(game);
            var command = blueprint.GenerateCommand(newGame); 
            command.Execute();
            if (!newGame.IsUnitPhaseAvailable(newGame.CurrentPhase))
                newGame.CycleTurn();

            return newGame;
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

