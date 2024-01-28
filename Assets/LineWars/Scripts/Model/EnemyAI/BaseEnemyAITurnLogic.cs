using System;
using System.Linq;
using System.Collections.Generic;

namespace LineWars.Model
{
    public abstract class BaseEnemyAITurnLogic
    {
        protected readonly EnemyAI ai;
        protected readonly GameEvaluator gameEvaluator;
        protected readonly DepthDetailsData depthDetailsData;

        public BaseEnemyAITurnLogic(
            EnemyAI ai, 
            GameEvaluator gameEvaluator, 
            DepthDetailsData depthDetailsData)
        {
            this.ai = ai;
            this.gameEvaluator = gameEvaluator;
            this.depthDetailsData = depthDetailsData;
        }

        public event Action Ended;

        public abstract void Start();

        protected void InvokeEnded()
        {
            Ended?.Invoke();
        }

        protected PossibleOutcome MinMax(GameProjection gameProjection, 
            ICommandBlueprint blueprint, 
            int depth,
            int currentExecutorId, 
            List<ICommandBlueprint> firstCommandChain, 
            bool isSavingCommands, int alpha = int.MinValue, int beta = int.MaxValue)
        {
            if (currentExecutorId != -1 && blueprint.ExecutorId != currentExecutorId)
                throw new ArgumentException();
            currentExecutorId = blueprint.ExecutorId;
            var newGame = GameProjectionCreator.FromProjection(gameProjection);
            var thisCommand = blueprint.GenerateCommand(newGame);
            thisCommand.Execute();

            if (isSavingCommands)
            {
                var newCommandChain = new List<ICommandBlueprint>(firstCommandChain);
                newCommandChain.Add(blueprint);
                firstCommandChain = newCommandChain;
            }

            var thisPlayerProjection = newGame.OriginalToProjectionPlayers[ai];
            if (newGame.UnitsIndexList.Count == 0)
                return new(gameEvaluator.Evaluate(newGame, thisPlayerProjection), firstCommandChain);

            if (IsTurnOver(newGame, currentExecutorId))
            {
                depth++;
                currentExecutorId = -1;
                isSavingCommands = false;
                if (!newGame.IsUnitPhaseAvailable())
                    newGame.CycleTurn();
                else
                    newGame.CyclePlayers();
            }
            if (newGame.CurrentPhase == PhaseType.Buy)
            {
                newGame.CycleTurn();
            }
            if (depth > ai.Depth || newGame.CurrentPhase == PhaseType.Buy)
            {
                return new(gameEvaluator.Evaluate(newGame, thisPlayerProjection), firstCommandChain);
            }

            var availableCommands = depthDetailsData.GetDepthDetails(depth).AvailableCommands;
            var possibleCommands = CommandBlueprintCollector.CollectAllCommands(newGame, availableCommands)
            .Where(newBlueprint => currentExecutorId == -1 || newBlueprint.ExecutorId == currentExecutorId);

            if (thisPlayerProjection != newGame.CurrentPlayer)
            {
                var currentValue = new PossibleOutcome(int.MaxValue, null);
                foreach (var command in possibleCommands)
                {
                    var newValue = MinMax(newGame, 
                        command, 
                        depth, 
                        currentExecutorId, 
                        firstCommandChain, 
                        isSavingCommands, 
                        alpha, beta);
                    if (newValue.Score < currentValue.Score)
                        currentValue = newValue;
                    if (newValue.Score <= alpha)
                    {
                        return currentValue;
                    }
                    if (newValue.Score < beta)
                        beta = newValue.Score;
                }
                return currentValue;
            }
            else
            {
                var currentValue = new PossibleOutcome(int.MinValue, null);
                foreach (var command in possibleCommands)
                {
                    var newValue = MinMax(newGame, 
                        command, 
                        depth, 
                        currentExecutorId, 
                        firstCommandChain, 
                        isSavingCommands, 
                        alpha, beta);
                    if (newValue.Score > currentValue.Score)
                        currentValue = newValue;
                    if (newValue.Score >= beta)
                    {
                        return currentValue;
                    }
                    if (newValue.Score > alpha)
                        alpha = newValue.Score;
                }
                return currentValue;
            }
        }

        private bool IsTurnOver(GameProjection game, int currentExecutorId)
        {
            if (game.CurrentPhase != PhaseType.Buy && currentExecutorId != -1)
            {
                if (!game.UnitsIndexList.ContainsKey(currentExecutorId))
                    return true;
                var currentExecutor = game.UnitsIndexList[currentExecutorId];
                return currentExecutor.CurrentActionPoints <= 0;
            }
            return true;
        }
    }
}
