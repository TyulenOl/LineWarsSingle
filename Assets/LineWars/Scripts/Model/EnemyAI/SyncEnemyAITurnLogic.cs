using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    public partial class EnemyAI
    {
        public class SyncEnemyAITurnLogic : BaseEnemyAITurnLogic
        {
            public SyncEnemyAITurnLogic(EnemyAI ai,
                GameEvaluator gameEvaluator,
                DepthDetailsData depthDetailsData) : base(ai, gameEvaluator, depthDetailsData)
            {
            }

            public override void Start()
            {
                var gameProjection = GameProjectionCreator.FromMono(
                    SingleGameRoot.Instance.AllPlayers.Values,
                    MonoGraph.Instance,
                    PhaseManager.Instance);
                var possibleCommands =
                        CommandBlueprintCollector.CollectAllCommands(
                        gameProjection,
                        depthDetailsData.GetDepthDetails(1).AvailableCommands);
                var allOutcomes = new List<PossibleOutcome>();
                //Debug.Log("---------------------");
                foreach (var command in possibleCommands)
                {
                    var commandChain = new List<ICommandBlueprint>();
                    var possibleOutcome = MinMax(gameProjection, command, 1, -1, commandChain, true);
                    allOutcomes.Add(possibleOutcome);
                    //Debug.Log($"{possibleOutcome.Score}, {possibleOutcome.Commands.First()}");
                }

                var bestOutcome = allOutcomes.MaxItem((i1, i2) => i1.Score.CompareTo(i2.Score));
                //Debug.Log($"Chosen: {bestOutcome.Score}, {bestOutcome.Commands.First()}");
                ai.StartCoroutine(PlayOutcome(bestOutcome, gameProjection));
            }
        }
    }
}
