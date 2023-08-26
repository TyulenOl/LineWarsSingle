using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class AttackEdgeActionData : EnemyActionData
    {
        public override void AddAllPossibleActions(List<EnemyAction> list, EnemyAI basePlayer, IExecutor executor)
        {
            if(executor is not Artillery distanceUnit) return; 
            var queue = new Queue<(Node, int)>();
            var nodeSet = new HashSet<Node>();
            var edgeSet = new HashSet<Edge>();
            queue.Enqueue((distanceUnit.Node, distanceUnit.CurrentActionPoints));
            nodeSet.Add(distanceUnit.Node);
            while (queue.Count > 0)
            {
                var currentNodeInfo = queue.Dequeue();
                if(currentNodeInfo.Item2 == 0) continue;
                var pointsAfterMove = distanceUnit.MovePointsModifier.Modify(currentNodeInfo.Item2);
                var pointsAfterAttack = distanceUnit.AttackPointsModifier.Modify(currentNodeInfo.Item2);
                if (pointsAfterAttack >= 0)
                {
                    foreach (var edge in currentNodeInfo.Item1.Edges)
                    {
                        if(edgeSet.Contains(edge)) continue;
                        
                    }
                }

                foreach (var neighbor in currentNodeInfo.Item1.GetNeighbors())
                {
                    if (nodeSet.Contains(neighbor)) continue;
                    var edge = neighbor.GetLine(currentNodeInfo.Item1);
                    
                    if (pointsAfterMove >= 0 &&
                        distanceUnit.CanMoveOnLineWithType(edge.LineType)
                        && Graph.CheckNodeForWalkability(neighbor, distanceUnit))
                    {
                        queue.Enqueue((neighbor, pointsAfterMove));
                        nodeSet.Add(neighbor);
                    }
                }
            }
        }
    }

    public class AttackEdgeAction : EnemyAction
    {
        public AttackEdgeAction(EnemyAI basePlayer, IExecutor executor, Node nodeToWalk, Edge edge) : base(basePlayer, executor)
        {
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
