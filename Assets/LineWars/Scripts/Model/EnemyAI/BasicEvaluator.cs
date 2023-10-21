using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Basic Evaluator", menuName = "EnemyAI/Evaluators/Basic Evaluator")]
    public class BasicEvaluator : GameEvaluator
    {
        [SerializeField] private int moneyMultiplier;
        [SerializeField] private int nodesMultiplier;
        [SerializeField] private int unitsMultiplier;
        public override int Evaluate(GameProjection projection, BasePlayerProjection player)
        {
            var moneyScore = 0;
            foreach(var enemy in projection.PlayersIndexList.Values)
            {
                if (enemy == player)
                {
                    moneyScore += player.CurrentMoney;
                    continue;
                }
                moneyScore -= enemy.CurrentMoney;
            }

            var nodeScore = 0;
            foreach(var node in projection.NodesIndexList.Values)
            {
                if(node.Owner == player)
                {
                    nodeScore++;
                    continue;
                }
                if (node.Owner != null) nodeScore--;
            }

            var unitScore = 0;
            foreach(var units in projection.UnitsIndexList.Values)
            {
                if (units.Owner != player)
                {
                    unitScore--;
                    continue;
                }
                unitScore++;
            }

            return nodeScore * nodesMultiplier + unitScore * unitsMultiplier + moneyScore * moneyMultiplier;
        }
    }
}
