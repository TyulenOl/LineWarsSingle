using LineWars.Extensions;
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
        [SerializeField] private float commandPause;

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
            var commandEvalList = new List<(int, List<ICommandBlueprint>)>();
            foreach(var command in possibleCommands)
            {
                var commandChain = new List<ICommandBlueprint>();
                commandEvalList.Add(MinMax(gameProjection, command, depth, -1, commandChain, true));
            }

            StartCoroutine(TurnCoroutine());

            IEnumerator TurnCoroutine()
            {
                var bestBlueprint = commandEvalList.Aggregate((i1, i2) => i1.Item1 > i2.Item1 ? i1 : i2);
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

        private (int, List<ICommandBlueprint>) MinMax(GameProjection gameProjection, ICommandBlueprint blueprint, int depth, 
            int currentExecutorId, List<ICommandBlueprint> firstCommandChain, bool isSavingCommands)
        {
            //проверить ид экзекутора
            if(currentExecutorId != -1 && blueprint.ExecutorId != currentExecutorId)
            {
                throw new ArgumentException();
            }
            // cгенерировать новую проекцию из комманды и обновиить ид экзекутора
            currentExecutorId = blueprint.ExecutorId;
            var newGame = GameProjection.GetCopy(gameProjection);
            var thisCommand = blueprint.GenerateCommand(newGame);
            thisCommand.Execute();
            //если сохран€ютс€ команды, то записать команду
            if(isSavingCommands)
            {
                var newCommandChain = new List<ICommandBlueprint>(firstCommandChain);
                newCommandChain.Add(blueprint);
                firstCommandChain = newCommandChain;
            }
            //проверить закончилс€ ли ход, если да, обнулить экзекутора, выключить сохранение комманд
            // а также проверить фазу, если фаза окончена, то найти новую фазу и игрока
            // если нет, то найти нового игрока
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
            //проверить закончилась ли глубина, если да, то оценить проекцию и return
            var thisPlayerProjection = newGame.OriginalToProjectionPlayers[this];
            if (depth == 0 || newGame.CurrentPhase == PhaseType.Buy)
            {
                return (gameEvaluator.Evaluate(newGame, thisPlayerProjection), firstCommandChain);
            }

            //пройтись по доступным командам и запустить на них минмакс
            //если текущий игрок == this, найти наивысшую, если != - найти наименьшую

            var possibleCommands = CommandBlueprintCollector.CollectAllCommands(newGame)
                .Where(newBlueprint => currentExecutorId == -1 || newBlueprint.ExecutorId != currentExecutorId)
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

