using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Basic Evaluator", menuName = "EnemyAI/Evaluators/Basic Evaluator")]
    public class BasicEvaluator : GameEvaluator
    {
        [SerializeField] private int moneyMultiplier;
        [SerializeField] private int nodesMultiplier;
        [SerializeField] private int HpMultiplier;
        [SerializeField] private int enemyHpMultiplier;
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
            foreach(var unit in projection.UnitsIndexList.Values)
            {
                if (unit.Owner != player)
                {
                    unitScore -= unit.CurrentHp * enemyHpMultiplier;
                    unitScore -= unit.CurrentArmor * enemyHpMultiplier;
                    continue;
                }
                unitScore += unit.CurrentHp * HpMultiplier;
                unitScore += unit.CurrentArmor * HpMultiplier;
            }
            //Debug.Log($"moneyS : {moneyScore * moneyMultiplier} hpS : {unitScore} nodeS : {nodeScore * nodesMultiplier}");
            return nodeScore * nodesMultiplier + unitScore + moneyScore * moneyMultiplier;
        }
    }
}
