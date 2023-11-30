using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace LineWars.Model
{
    public partial class EnemyAI
    {
        public class EnemyAITurnLogic
        {
            private readonly EnemyAI ai;

            public EnemyAITurnLogic(EnemyAI ai)
            {
                this.ai = ai;
            }

            public event Action Ended;

            public void Start()
            {
                ai.StartCoroutine(StartAITurnCoroutine());
            }

            private IEnumerator StartAITurnCoroutine()
            {
                var gameProjection =
                    GameProjectionCreator.FromMono(SingleGame.Instance.AllPlayers.Values, MonoGraph.Instance, PhaseManager.Instance);
                var allCommmandsTask = FindAllOutcomes(gameProjection);
                allCommmandsTask.Start();
                yield return new WaitUntil(() => allCommmandsTask.IsCompleted);
                var commandEvalList = allCommmandsTask.Result;

                yield return ExecuteTurnCoroutine(commandEvalList, gameProjection);
            }

            private IEnumerator ExecuteTurnCoroutine(PossibleOutcome[] commandEvalList, GameProjection gameProjection)
            {
                yield return new WaitForSeconds(ai.firstCommandPause);
                var bestBlueprint = commandEvalList.MaxItem((i1, i2) => i1.Score.CompareTo(i2.Score));
                foreach (var blueprint in bestBlueprint.Commands)
                {
                    var command = blueprint.GenerateMonoCommand(gameProjection);
                    UnitsController.ExecuteCommand(command);
                    yield return new WaitForSeconds(ai.commandPause);
                }

                Ended?.Invoke();
            }

            private Task<PossibleOutcome[]> FindAllOutcomes(GameProjection gameProjection)
            {
                var possibleCommands = CommandBlueprintCollector.CollectAllCommands(gameProjection);
                var tasksList = new List<Task<PossibleOutcome>>();
                foreach (var command in possibleCommands)
                {
                    var commandChain = new List<ICommandBlueprint>();
                    tasksList.Add(ExploreOutcome(gameProjection, command, ai.Depth, -1, commandChain, true));
                }

                return Task.WhenAll(tasksList.ToArray());
            }


            private Task<PossibleOutcome> ExploreOutcome(GameProjection gameProjection, ICommandBlueprint blueprint, int depth,
                int currentExecutorId, List<ICommandBlueprint> firstCommandChain, bool isSavingCommands)
            {
                var task = new Task<PossibleOutcome>(
                    () => MinMax(gameProjection, blueprint, depth, currentExecutorId, firstCommandChain, isSavingCommands));
                //task.Start();
                return task;
            }

            private PossibleOutcome MinMax(GameProjection gameProjection, ICommandBlueprint blueprint, int depth,
                int currentExecutorId, List<ICommandBlueprint> firstCommandChain, bool isSavingCommands)
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
                    return new(ai.gameEvaluator.Evaluate(newGame, thisPlayerProjection), firstCommandChain);

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
                    return new(ai.gameEvaluator.Evaluate(newGame, thisPlayerProjection), firstCommandChain);
                }

                var possibleCommands = CommandBlueprintCollector.CollectAllCommands(newGame)
                .Where(newBlueprint => currentExecutorId == -1 || newBlueprint.ExecutorId == currentExecutorId)
                .Select(newBlueprint => MinMax(newGame, newBlueprint, depth, currentExecutorId, firstCommandChain, isSavingCommands));

                if (thisPlayerProjection != newGame.CurrentPlayer)
                {
                    var minChain = possibleCommands.MinItem((i1, i2) => i1.Score.CompareTo(i2.Score));
                    return minChain;
                }
                else
                {
                    var maxChain = possibleCommands.MaxItem((i1, i2) => i1.Score.CompareTo(i2.Score));
                    return maxChain;
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
}
