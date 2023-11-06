using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Advanced Evaluator", menuName = "EnemyAI/Evaluators/Advanced Evaluator")]
    public class AdvancedEvaluator : GameEvaluator
    {
        [Header("Node Settings")]
        [SerializeField] private int ourNodeMultiplier;
        [SerializeField] private int enemyNodeMultiplier;

        [Header("Node Visibility Settings")]
        [SerializeField] private int ourVisibleOwnedNodeMultiplier;
        [SerializeField] private int ourVisibleNotOwnedNodeMultiplier;

        [SerializeField] private int enemyVisibleOwnedNodeMultiplier;
        [SerializeField] private int enemyVisibleNotOwnedNodeMultiplier;

        [Header("Hp and Armor Settings")]
        [SerializeField] private int ourHpMultiplier;
        [SerializeField] private int enemyHpMultiplier;

        [Header("Units Settings")]
        [SerializeField] private int ourUnitsMultiplier;
        [SerializeField] private int enemyUnitsMultiplier;
        

        public override int Evaluate(GameProjection projection, BasePlayerProjection player)
        {
            var resultScore = 0;
            foreach(var thisPlayer in projection.PlayersIndexList.Values)
            {
                if (thisPlayer == player)
                    resultScore += EvaluateScoreForPlayer(projection, player, ourNodeMultiplier, ourHpMultiplier,
                        ourVisibleOwnedNodeMultiplier, ourVisibleNotOwnedNodeMultiplier, ourUnitsMultiplier);
                else
                    resultScore += EvaluateScoreForPlayer(projection, thisPlayer, enemyNodeMultiplier, enemyHpMultiplier,
                        enemyVisibleOwnedNodeMultiplier, enemyVisibleNotOwnedNodeMultiplier, enemyUnitsMultiplier);
            }

            return resultScore;
            
        }

        private int EvaluateScoreForPlayer(GameProjection projection, BasePlayerProjection player,
            int nodeMultiplier, int hpMultiplier, int visibleOwnedNodeMultiplier, int visibleNotOwnedNodeMultiplier,
            int unitMultiplier)
        {
            var resultScore = 0;

            foreach(var node in player.OwnedObjects.OfType<NodeProjection>())
            {
                resultScore += node.Score * nodeMultiplier;
            }

            foreach(var unit in player.OwnedObjects.OfType<UnitProjection>())
            {
                resultScore += unit.CurrentHp * hpMultiplier;
                resultScore += unit.MaxArmor * hpMultiplier;

                foreach(var node in projection.Graph.GetNodesInRange(unit.Node, 1))
                {
                    if (node.Owner == player)
                        resultScore += visibleOwnedNodeMultiplier * node.Score;
                    else
                        resultScore += visibleNotOwnedNodeMultiplier * node.Score;
                }

                resultScore += unitMultiplier;
            }

            return resultScore;
        }
    }
}
