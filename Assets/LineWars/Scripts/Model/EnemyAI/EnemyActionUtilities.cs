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
    }
}

