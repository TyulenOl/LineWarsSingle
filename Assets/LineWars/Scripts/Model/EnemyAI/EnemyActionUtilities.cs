// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace LineWars.Model
// {
//     public static class EnemyActionUtilities
//     {
//         public static List<ComponentUnit> FindAdjacentEnemies(Node node, BasePlayer basePlayer)
//         {
//             var enemies = new List<ComponentUnit>();
//             foreach (var edge in node.Edges)
//             {
//                 var otherNode = edge.FirstNode == node ? edge.SecondNode : edge.FirstNode;
//                 if (otherNode.LeftUnit != null && otherNode.LeftUnit.Owner != basePlayer)
//                     enemies.Add(otherNode.LeftUnit);
//                 if (otherNode.RightUnit != null && otherNode.RightUnit.Owner != basePlayer)
//                     enemies.Add(otherNode.RightUnit);
//             }
//
//             return enemies;
//         }
//         
//         public static List<ComponentUnit> FindAdjacentEnemies(Node node, BasePlayer basePlayer, LineType minEdgeType)
//         {
//             var enemies = new List<ComponentUnit>();
//             foreach (var edge in node.Edges)
//             {
//                 if((int) minEdgeType > (int)edge.LineType) continue;
//                 var otherNode = edge.FirstNode == node ? edge.SecondNode : edge.FirstNode;
//                 if (otherNode.LeftUnit != null && otherNode.LeftUnit.Owner != basePlayer)
//                     enemies.Add(otherNode.LeftUnit);
//                 if (otherNode.RightUnit != null && otherNode.RightUnit.Owner != basePlayer)
//                     enemies.Add(otherNode.RightUnit);
//             }
//
//             return enemies;
//         }
//         
//         public static List<ComponentUnit> FindAdjacentAllies(Node node, BasePlayer basePlayer, LineType minEdgeType)
//         {
//             var allies = new List<ComponentUnit>();
//             foreach (var edge in node.Edges)
//             {
//                 if((int) minEdgeType > (int)edge.LineType) continue;
//                 var otherNode = edge.FirstNode == node ? edge.SecondNode : edge.FirstNode;
//                 if (otherNode.LeftUnit != null && otherNode.LeftUnit.Owner == basePlayer)
//                     allies.Add(otherNode.LeftUnit);
//                 if (otherNode.RightUnit != null && otherNode.RightUnit.Owner == basePlayer)
//                     allies.Add(otherNode.RightUnit);
//             }
//
//             return allies;
//         }
//
//         public static List<ComponentUnit> GetUnitsInNode(Node node)
//         {
//             var units = new List<ComponentUnit>();
//             if (node.LeftUnit != null)
//                 units.Add(node.LeftUnit);
//             if (node.RightUnit != null)
//                 units.Add(node.RightUnit);
//
//             return units;
//         }
//
//         public static List<Node> GetNodesInIntModifierRange(Node node, int range, IntModifier modifier)
//         {
//             var list = new List<Node>();
//             var queue = new Queue<(Node, int)>();
//             var nodeSet = new HashSet<Node>();
//             
//             queue.Enqueue((node, range));
//             while (queue.Count > 0)
//             {
//                 var currentNodeInfo = queue.Dequeue();
//                 if(currentNodeInfo.Item2 <= 0) continue;
//                 foreach (var neighborNode in currentNodeInfo.Item1.GetNeighbors())
//                 {
//                     if(nodeSet.Contains(neighborNode)) continue;
//                     var rangeAfterMove = modifier.Modify(currentNodeInfo.Item2);
//                     if (rangeAfterMove >= 0)
//                     {
//                         list.Add(neighborNode);
//                         queue.Enqueue((neighborNode, rangeAfterMove));
//                         nodeSet.Add(neighborNode);
//                     }
//                 }
//             }
//
//             return list;
//         }
//         
//         public static List<Node> GetNodesInIntModifierRange(Node node, int range, 
//             IntModifier modifier, Action<Node, Node, int> nodeParser)
//         {
//             var list = new List<Node>();
//             var queue = new Queue<(Node, int)>();
//             var nodeSet = new HashSet<Node>();
//             
//             queue.Enqueue((node, range));
//             while (queue.Count > 0)
//             {
//                 var currentNodeInfo = queue.Dequeue();
//                 if(currentNodeInfo.Item2 <= 0) continue;
//                 foreach (var neighborNode in currentNodeInfo.Item1.GetNeighbors())
//                 {
//                     if(nodeSet.Contains(neighborNode)) continue;
//                     var rangeAfterMove = modifier.Modify(currentNodeInfo.Item2);
//                     if (rangeAfterMove >= 0)
//                     {
//                         list.Add(neighborNode);
//                         queue.Enqueue((neighborNode, rangeAfterMove));
//                         nodeSet.Add(neighborNode);
//                         nodeParser(currentNodeInfo.Item1, neighborNode, rangeAfterMove);
//                     }
//                 }
//             }
//
//             return list;
//         }
//         
//         public static List<Node> GetNodesInIntModifierRange(Node node, int range, 
//             IntModifier modifier, Action<Node, Node, int> nodeParser, ComponentUnit unit)
//         {
//             var list = new List<Node>();
//             var queue = new Queue<(Node, int)>();
//             var nodeSet = new HashSet<Node>();
//             
//             queue.Enqueue((node, range));
//             nodeParser(null, node, range);
//             while (queue.Count > 0)
//             {
//                 var currentNodeInfo = queue.Dequeue();
//                 if(currentNodeInfo.Item2 <= 0) continue;
//                 foreach (var neighborNode in currentNodeInfo.Item1.GetNeighbors())
//                 {
//                     if(nodeSet.Contains(neighborNode)) continue;
//                     var rangeAfterMove = modifier.Modify(currentNodeInfo.Item2);
//                     var edge = neighborNode.GetLine(currentNodeInfo.Item1);
//                     if (rangeAfterMove >= 0 
//                         && unit.CanMoveOnLineWithType(edge.LineType)
//                         && Graph.CheckNodeForWalkability(neighborNode, unit))
//                     {
//                         list.Add(neighborNode);
//                         queue.Enqueue((neighborNode, rangeAfterMove));
//                         nodeSet.Add(neighborNode);
//                         nodeParser(currentNodeInfo.Item1, neighborNode, rangeAfterMove);
//                     }
//                 }
//             }
//
//             return list;
//         }
//     }
// }
//
