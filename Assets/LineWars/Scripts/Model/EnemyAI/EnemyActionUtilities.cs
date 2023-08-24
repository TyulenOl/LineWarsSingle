using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public static class EnemyActionUtilities
    {
        public static List<Unit> FindAdjacentEnemies(Node node, BasePlayer basePlayer)
        {
            var enemies = new List<Unit>();
            foreach (var edge in node.Edges)
            {
                var otherNode = edge.FirstNode == node ? edge.SecondNode : edge.FirstNode;
                if (otherNode.LeftUnit != null && otherNode.LeftUnit.Owner != basePlayer)
                    enemies.Add(otherNode.LeftUnit);
                if (otherNode.RightUnit != null && otherNode.RightUnit.Owner != basePlayer)
                    enemies.Add(otherNode.RightUnit);
            }

            return enemies;
        }

        public static List<Unit> GetUnitsInNode(Node node)
        {
            var units = new List<Unit>();
            if (node.LeftUnit != null)
                units.Add(node.LeftUnit);
            if (node.RightUnit != null)
                units.Add(node.RightUnit);

            return units;
        }

        public static List<Node> GetNodesInIntModifierRange(Node node, int range, IntModifier modifier)
        {
            var list = new List<Node>();
            var queue = new Queue<(Node, int)>();
            var nodeSet = new HashSet<Node>();
            
            queue.Enqueue((node, range));
            while (queue.Count > 0)
            {
                var currentNodeInfo = queue.Dequeue();
                if(currentNodeInfo.Item2 <= 0) continue;
                foreach (var neighborNode in currentNodeInfo.Item1.GetNeighbors())
                {
                    if(nodeSet.Contains(neighborNode)) continue;
                    var rangeAfterMove = modifier.Modify(currentNodeInfo.Item2);
                    if (rangeAfterMove >= 0)
                    {
                        list.Add(neighborNode);
                        queue.Enqueue((neighborNode, rangeAfterMove));
                        nodeSet.Add(neighborNode);
                    }
                }
            }

            return list;
        }
    }
}

