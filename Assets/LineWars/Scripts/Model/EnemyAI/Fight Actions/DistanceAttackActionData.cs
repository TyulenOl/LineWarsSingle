using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LineWars.Model
{
    public class DistanceAttackActionData : EnemyActionData
    {
        [SerializeField] private IntModifier distanceAttackModifier;
        public override void AddAllPossibleActions(List<EnemyAction> list, EnemyAI basePlayer, IExecutor executor)
        {
            if (executor is not Artillery distanceUnit) return;

            var queue = new Queue<(Node, int)>();
            var nodeSet = new HashSet<Node>();
            var enemySet = new HashSet<Unit>();
            
            queue.Enqueue((distanceUnit.Node, distanceUnit.CurrentActionPoints));
            nodeSet.Add(distanceUnit.Node);
            while (queue.Count >= 0)
            {
                var currentNodeInfo = queue.Dequeue();
                if(currentNodeInfo.Item2 == 0) continue;

                var enemies = CheckDistanceAttack(currentNodeInfo.Item1, distanceUnit, enemySet);
                var pointsAfterAttack = distanceUnit.AttackPointsModifier.Modify(currentNodeInfo.Item2);
                if(pointsAfterAttack >= 0)
                    foreach (var enemy in enemies)
                    {
                        list.Add(new DistanceAttackAction(basePlayer, executor, currentNodeInfo.Item1, enemy));
                    }
                
                foreach (var neighborNode in currentNodeInfo.Item1.GetNeighbors())
                {
                    if (nodeSet.Contains(neighborNode)) continue;

                    var pointsAfterMove = distanceUnit.MovePointsModifier.Modify(currentNodeInfo.Item2);

                    var edge = neighborNode.GetLine(currentNodeInfo.Item1);
                    if (pointsAfterMove >= 0 &&
                        distanceUnit.CanMoveOnLineWithType(edge.LineType)
                        && Graph.CheckNodeForWalkability(neighborNode, distanceUnit))
                    {
                        queue.Enqueue((currentNodeInfo.Item1, pointsAfterMove));
                        nodeSet.Add(currentNodeInfo.Item1);
                    }
                }
            }
        }

        private List<Unit> CheckDistanceAttack(Node node, Artillery distanceUnit, HashSet<Unit> enemySet)
        {
            var list = new List<Unit>();
            var attackedNodes =
                EnemyActionUtilities.GetNodesInIntModifierRange(node,
                    distanceUnit.Distance,
                    distanceAttackModifier);
            foreach (var attackedNode in attackedNodes)
            {
                var unitsInNode = EnemyActionUtilities.GetUnitsInNode(attackedNode);
                foreach (var unit in unitsInNode)
                {
                    if(enemySet.Contains(unit)) continue;
                    if(unit.Owner == distanceUnit.Owner) continue;
                    if(distanceUnit.CanAttack(unit))
                        list.Add(unit);
                }
            }

            return list;
        }
    }
    
    public class DistanceAttackAction : EnemyAction
    {
        private readonly Node nodeToWalk;
        private readonly Unit target;
        private readonly Artillery distanceUnit;
        private readonly List<Node> path;
        public DistanceAttackAction(EnemyAI basePlayer, IExecutor executor, Node node, Unit targetUnit) : base(basePlayer, executor)
        {
            if (executor is not Artillery distanceUnit)
            {
                Debug.LogError($"{executor} is not a DistanceUnit!");
                return;
            }

            this.distanceUnit = distanceUnit;
            nodeToWalk = node;
            target = targetUnit;
            path = Graph.FindShortestPath(distanceUnit.Node, nodeToWalk, this.distanceUnit);
            score = GetScore();
        }

        public override void Execute()
        {
            
        }

        private float GetScore()
        {
            return 1;
        }
    }
    
    
}
