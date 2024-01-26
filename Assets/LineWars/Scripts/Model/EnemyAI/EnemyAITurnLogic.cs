using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace LineWars.Model
{
    public partial class EnemyAI
    {
        private const string Prefix = "ENEMY AI";
        public class EnemyAITurnLogic : BaseEnemyAITurnLogic
        {
            private int comCount;

            public EnemyAITurnLogic(EnemyAI ai, 
                GameEvaluator gameEvaluator, 
                DepthDetailsData depthDetailsData) : base(ai, gameEvaluator, depthDetailsData)
            {
            }

            public override void Start()
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

                InvokeEnded();
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

            private void DebugBullshit(GameProjection game)
            {
                UnityEngine.Debug.Log($"{game.UnitsIndexList.Count} Units");
                foreach (var unit in game.UnitsIndexList.Values)
                {
                    if (unit.CurrentHp <= 0)
                        UnityEngine.Debug.Log("you suck!");
                }
            }
        }
    }
}
