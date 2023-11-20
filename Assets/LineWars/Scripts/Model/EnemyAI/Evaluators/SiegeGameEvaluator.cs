using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Siege Evaluator", menuName = "EnemyAI/Evaluators/Siege")]
    public class SiegeGameEvaluator : GameEvaluator
    {
        [SerializeField] private int enemiesHpPoints;

        [Header("Center Options")]
        [SerializeField] private int pointsForDistanceToCenter;
        [SerializeField] private int centerId;
        [Header("Enemy Proximity")]
        [SerializeField] private int searchRadius = 1;
        [SerializeField] private int pointsForEnemyProximity;
       
        public override int Evaluate(GameProjection projection, BasePlayerProjection player)
        {
            var hpPoints = 0;
            var proximityPoints = 0;
            var centerPoints = 0;
            foreach(var unit in projection.UnitsIndexList.Values)
            {
                if (unit.OwnerId != player.Id)
                    ProccessEnemyUnit(unit);
                else
                    ProccessOurUnit(unit);
            }

            return hpPoints + proximityPoints + centerPoints;

            void ProccessEnemyUnit(UnitProjection unit)
            {
                hpPoints += unit.CurrentHp * enemiesHpPoints;
                hpPoints += unit.CurrentArmor * enemiesHpPoints;
            }

            void ProccessOurUnit(UnitProjection unit)
            {
                var closeNodes = projection.Graph.GetNodesInRange(unit.Node, (uint)searchRadius);
                foreach (var node in closeNodes)
                {
                    if (node.LeftUnit != null && node.LeftUnit.OwnerId != player.Id)
                        proximityPoints += pointsForEnemyProximity;
                    if (node.RightUnit != null && node.RightUnit.OwnerId != player.Id)
                        proximityPoints += pointsForEnemyProximity;
                }

                var distanceToCenter = projection.Graph.FindShortestPath(unit.Node, projection.NodesIndexList[centerId]).Count;
                centerPoints +=  distanceToCenter * pointsForDistanceToCenter;
            }
        }
    }
}
