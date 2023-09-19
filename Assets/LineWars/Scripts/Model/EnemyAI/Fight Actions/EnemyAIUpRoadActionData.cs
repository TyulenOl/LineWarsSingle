// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEditor;
// using UnityEngine;
//
// namespace LineWars.Model
// {
//     [CreateAssetMenu(fileName = "New Up Road Action", menuName = "EnemyAI/Enemy Actions/Fight Phase/Up Road")]
//     public class EnemyAIUpRoadActionData : EnemyActionData
//     {
//         [SerializeField] private float waitTime;
//         [SerializeField] private float baseScore;
//         [SerializeField] private float bonusForFirstUnit;
//         [SerializeField] private float bonusForNextUnits;
//         [SerializeField] private float bonusForSameRoadAdjacency;
//         [SerializeField] private float bonusPerPoints;
//
//         public float WaitTime => waitTime;
//         public float BaseScore => baseScore;
//         public float BonusForFirstUnit => bonusForFirstUnit;
//         public float BonusForNextUnits => bonusForNextUnits;
//         public float BonusForSameRoadAdjacency => bonusForSameRoadAdjacency;
//         public float BonusPerPoints => bonusPerPoints;
//
//         public override void AddAllPossibleActions(List<EnemyAction> list, EnemyAI basePlayer, IExecutor executor)
//         {
//             if (executor is not Engineer engineer) return;
//             var edgeSet = new HashSet<Edge>();
//             EnemyActionUtilities.GetNodesInIntModifierRange(engineer.Node, engineer.CurrentActionPoints,
//                 engineer.MovePointsModifier,
//                 ((prevNode, node, range) => NodeParser(prevNode, node, range, engineer, edgeSet, basePlayer, list)),
//                 engineer);
//         }
//
//         private void NodeParser(Node _, Node node, int actionPoints, Engineer engineer,
//             HashSet<Edge> edgeSet, EnemyAI basePlayer, List<EnemyAction> actionList)
//         {
//             if (actionPoints <= 0) return;
//             var pointsAfterUpRoad = engineer.EngineerPointModifier.Modify(actionPoints);
//             if (pointsAfterUpRoad < 0) return;
//             foreach (var edge in node.Edges)
//             {
//                 if (edgeSet.Contains(edge)) continue;
//                 edgeSet.Add(edge);
//                 if (!engineer.CanUpRoad(edge, node)) continue;
//                 actionList.Add(new UpRoadAction(basePlayer, engineer, node, edge, this));
//             }
//         }
//     }
//
//     public class UpRoadAction : EnemyAction
//     {
//         private readonly Engineer engineer;
//         private readonly Node node;
//         private readonly Edge edge;
//         private readonly List<Node> path;
//         private readonly EnemyAIUpRoadActionData data;
//
//         public UpRoadAction(EnemyAI basePlayer, IExecutor executor, Node node, Edge edge, EnemyAIUpRoadActionData data)
//             : base(basePlayer, executor)
//         {
//             if (executor is not Engineer engineer)
//             {
//                 Debug.LogError($"{executor} is not Engineer!");
//                 return;
//             }
//
//             this.node = node;
//             this.edge = edge;
//             this.engineer = engineer;
//             this.data = data;
//             path = Graph.FindShortestPath(engineer.Node, node, engineer);
//             path.Remove(engineer.Node);
//             score = GetScore();
//         }
//
//         public override void Execute()
//         {
//             basePlayer.StartCoroutine(ExecuteCoroutine());
//
//             IEnumerator ExecuteCoroutine()
//             {
//                 foreach (var node in path)
//                 {
//                     if (engineer.Node == node) continue;
//                     if (!engineer.CanMoveTo(node))
//                         Debug.LogError($"{engineer} cannot move to {node}");
//                     UnitsController.ExecuteCommand(new UnitMoveCommand(engineer, node));
//                     yield return new WaitForSeconds(data.WaitTime);
//                 }
//
//                 if (!engineer.CanUpRoad(edge))
//                 {
//                     Debug.LogError($"{engineer} cannot up {edge}");
//                     yield break;
//                 }
//
//                 UnitsController.ExecuteCommand(new UnitUpRoadCommand(engineer, edge));
//                 InvokeActionCompleted();
//             }
//         }
//
//         private float GetScore()
//         {
//             var finalScore = data.BaseScore;
//             var upgradedType = LineTypeHelper.Up(edge.LineType);
//             var enemiesCount = basePlayer.OwnedObjects
//                 .Where((owned => owned is ComponentUnit unit && unit.MovementLineType == upgradedType))
//                 .Count();
//             if (enemiesCount >= 1)
//                 finalScore += data.BonusForFirstUnit;
//             if (enemiesCount > 1)
//                 finalScore += (enemiesCount - 1) * data.BonusForNextUnits;
//
//             var edgesCount = node.Edges
//                 .Where(((edge1) => edge.LineType == upgradedType))
//                 .Count();
//             if (edgesCount > 0)
//                 finalScore += data.BonusForSameRoadAdjacency;
//
//             var pointsLeft = engineer.CurrentActionPoints;
//             foreach (var nextNode in path)
//             {
//                 if (nextNode == engineer.Node) continue;
//                 pointsLeft = engineer.MovePointsModifier.Modify(pointsLeft);
//             }
//
//             pointsLeft = engineer.EngineerPointModifier.Modify(pointsLeft);
//             finalScore += pointsLeft * data.BonusPerPoints;
//             return finalScore;
//         }
//     }
// }