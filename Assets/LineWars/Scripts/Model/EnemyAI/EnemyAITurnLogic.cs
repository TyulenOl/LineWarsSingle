using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LineWars.Model
{
    public partial class EnemyAI
    {
        private const string Prefix = "ENEMY AI";
        public class EnemyAITurnLogic
        {
            private readonly EnemyAI ai;
            private int comCount;

            public EnemyAITurnLogic(EnemyAI ai)
            {
                this.ai = ai;
            }

            public event Action Ended;

            public void Start()
            {
                StartAITurn();
            }

            private async void StartAITurn()
            {
                var gameProjection = GameProjectionCreator.FromMono(SingleGameRoot.Instance.AllPlayers.Values, MonoGraph.Instance, PhaseManager.Instance);
                //DEBUG
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var allCommands = await FindAllOutcomes(gameProjection);
                stopwatch.Stop();
                UnityEngine.Debug.Log($"{stopwatch.ElapsedMilliseconds} ms");

                ai.StartCoroutine(ExecuteTurnCoroutine(allCommands, gameProjection));
            }

            private IEnumerator ExecuteTurnCoroutine(PossibleOutcome[] commandEvalList, GameProjection gameProjection)
            {
                Debug.Log($"{Prefix} Execute Turn Coroutine");
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
                Debug.Log($"{Prefix} Find All Outcomes");
                var possibleCommands = 
                    CommandBlueprintCollector.CollectAllCommands(
                    gameProjection,
                    ai.depthDetailsData.GetDepthDetails(1).AvailableCommands);
                Debug.Log($"{Prefix} PossibleCommandsCount: {possibleCommands.Count}");
                var tasksList = new List<Task<PossibleOutcome>>();
                foreach (var command in possibleCommands)
                {
                    var commandChain = new List<ICommandBlueprint>();
                    tasksList.Add(ExploreOutcome(gameProjection, command, 1, -1, commandChain, true));
                }

                return Task.WhenAll(tasksList.ToArray());
            }

            private Task<PossibleOutcome> ExploreOutcome(GameProjection gameProjection, ICommandBlueprint blueprint, int depth,
                int currentExecutorId, List<ICommandBlueprint> firstCommandChain, bool isSavingCommands)
            {
                Debug.Log($"{Prefix} Explore Outcome");
                var task = new Task<PossibleOutcome>(
                    () => MinMax(gameProjection, blueprint, depth, currentExecutorId, firstCommandChain, isSavingCommands));
                task.Start();
                return task;
            }

            private PossibleOutcome MinMax(GameProjection gameProjection, ICommandBlueprint blueprint, int depth,
                int currentExecutorId, List<ICommandBlueprint> firstCommandChain, bool isSavingCommands, int alpha = int.MinValue, int beta = int.MaxValue)
            {
                Debug.Log($"{Prefix} Start MinMax");
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
                    depth++;
                    currentExecutorId = -1;
                    isSavingCommands = false;
                    if (!newGame.IsUnitPhaseAvailable())
                        newGame.CycleTurn();
                    else
                        newGame.CyclePlayers();
                }
                if(newGame.CurrentPhase == PhaseType.Buy)
                {
                    newGame.CycleTurn();
                }
                if (depth > ai.Depth || newGame.CurrentPhase == PhaseType.Buy)
                {
                    return new(ai.gameEvaluator.Evaluate(newGame, thisPlayerProjection), firstCommandChain);
                }

                var availableCommands = ai.depthDetailsData.GetDepthDetails(depth).AvailableCommands;
                var possibleCommands = CommandBlueprintCollector.CollectAllCommands(newGame, availableCommands)
                .Where(newBlueprint => currentExecutorId == -1 || newBlueprint.ExecutorId == currentExecutorId);

                if (thisPlayerProjection != newGame.CurrentPlayer)
                {
                    var currentValue = new PossibleOutcome(int.MaxValue, null);
                    foreach (var command in possibleCommands)
                    {
                        var newValue = MinMax(newGame, command, depth, currentExecutorId, firstCommandChain, isSavingCommands, alpha, beta);
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
                    foreach(var command in possibleCommands)
                    {
                        var newValue = MinMax(newGame, command, depth, currentExecutorId, firstCommandChain, isSavingCommands, alpha, beta);
                        if(newValue.Score > currentValue.Score)
                            currentValue = newValue;
                        if(newValue.Score >= beta)
                        { 
                            return currentValue;
                        }
                        if (newValue.Score > alpha)
                            alpha = newValue.Score;
                    }
                    return currentValue;
                }
            }

            private void DebugBullshit(GameProjection game)
            {
                UnityEngine.Debug.Log($"{game.UnitsIndexList.Count} Units");
                foreach (var unit in game.UnitsIndexList.Values)
                {
                    if (unit.CurrentHp <= 0)
                        UnityEngine.Debug.Log("you suck!");
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
