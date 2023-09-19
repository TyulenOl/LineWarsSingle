// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
//
// namespace LineWars.Model
// {
//     [CreateAssetMenu(fileName = "New Attack Edge Action", menuName = "EnemyAI/Enemy Actions/Fight Phase/Attack Edge")]
//     public class EnemyAIAttackEdgeActionData : EnemyActionData
//     {
//         [SerializeField] private IntModifier distanceAttackModifier;
//         [SerializeField] private float waitTime;
//         [SerializeField] private float baseScore;
//         [SerializeField] private float bonusPerFirstEnemyUnit;
//         [SerializeField] private float bonusPerNextEnemyUnits;
//         [SerializeField] private float bonusPerPoints;
//
//         public float WaitTime => waitTime;
//         public float BaseScore => baseScore;
//         public float BonusPerFirstEnemyUnit => bonusPerFirstEnemyUnit;
//         public float BonusPerNextEnemyUnits => bonusPerNextEnemyUnits;
//         public float BonusPerPoints => bonusPerPoints;
//
//         public override void AddAllPossibleActions(List<EnemyAction> actionList, EnemyAI basePlayer, IExecutor executor)
//         {
//             if (executor is not Artillery artillery) return;
//             var edgeSet = new HashSet<Edge>();
//             NodeParser(null, artillery.Node, artillery.CurrentActionPoints,
//                 artillery, edgeSet, actionList, basePlayer);
//
//             EnemyActionUtilities.GetNodesInIntModifierRange(artillery.Node,
//                 artillery.CurrentActionPoints, distanceAttackModifier,
//                 (prevNode, node, actionPoints) =>
//                     NodeParser(prevNode, node, actionPoints, artillery, edgeSet, actionList, basePlayer), artillery);
//         }
//
//         private void NodeParser(Node _, Node node, int actionPoints, Artillery artillery, HashSet<Edge> edgeSet,
//             List<EnemyAction> actionList, EnemyAI basePlayer)
//         {
//             var edgeList = CheckDistanceAttack(node, artillery, edgeSet);
//             var pointsAfterAttack = artillery.AttackPointsModifier.Modify(actionPoints);
//             if (actionPoints == 0 || pointsAfterAttack < 0) return;
//             foreach (var edge in edgeList)
//             {
//                 actionList.Add(new AttackEdgeAction(basePlayer, artillery, node, edge, this));
//             }
//         }
//
//         private List<Edge> CheckDistanceAttack(Node node, Artillery artillery, HashSet<Edge> edgeSet)
//         {
//             var edgeList = new List<Edge>();
//             var attackedNodes =
//                 EnemyActionUtilities.GetNodesInIntModifierRange(node, artillery.Distance, distanceAttackModifier);
//
//             foreach (var attackedNode in attackedNodes)
//             {
//                 foreach (var edge in attackedNode.Edges)
//                 {
//                     if (edgeSet.Contains(edge)) continue;
//                     edgeSet.Add(edge);
//                     if (artillery.CanAttack(edge, attackedNode))
//                         edgeList.Add(edge);
//                 }
//             }
//
//             return edgeList;
//         }
//     }
//
//     public class AttackEdgeAction : EnemyAction
//     {
//         private readonly EnemyAIAttackEdgeActionData data;
//         private readonly Artillery artillery;
//         private readonly Node node;
//         private readonly Edge edge;
//         private readonly List<Node> path;
//
//         public AttackEdgeAction(EnemyAI basePlayer, IExecutor executor, Node nodeToWalk, Edge edge,
//             EnemyAIAttackEdgeActionData data) : base(basePlayer, executor)
//         {
//             if (executor is not Artillery artillery)
//             {
//                 Debug.LogError($"{base.executor} is not a Artillery!");
//                 return;
//             }
//
//             this.artillery = artillery;
//             this.node = nodeToWalk;
//             this.edge = edge;
//             path = Graph.FindShortestPath(artillery.Node, node, artillery);
//             path.Remove(this.artillery.Node);
//             this.data = data;
//             score = GetScore();
//         }
//
//         public override void Execute()
//         {
//             basePlayer.StartCoroutine(ExecuteCoroutine());
//
//             IEnumerator ExecuteCoroutine()
//             {
//                 foreach (var nextNode in path)
//                 {
//                     if (artillery.Node == nextNode) continue;
//                     if (!artillery.CanMoveTo(nextNode))
//                         Debug.LogError($"{artillery} cannot move to {nextNode}");
//                     UnitsController.ExecuteCommand(new UnitMoveCommand(artillery, nextNode));
//                     yield return new WaitForSeconds(data.WaitTime);
//                 }
//
//                 if (!artillery.CanAttack(edge))
//                 {
//                     Debug.LogError($"{artillery} cannot attack {edge}");
//                     yield break;
//                 }
//
//                 UnitsController.ExecuteCommand(new UnitAttackCommand(artillery, edge));
//                 InvokeActionCompleted();
//             }
//         }
//
//         private float GetScore()
//         {
//             var finalScore = data.BaseScore;
//             var enemiesCount = 0;
//             foreach (var enemyPlayer in SingleGame.Instance.AllPlayers)
//             {
//                 enemiesCount += enemyPlayer.OwnedObjects
//                     .Where((owned => owned is ComponentUnit unit && unit.MovementLineType == edge.LineType))
//                     .Count();
//             }
//
//             if (enemiesCount >= 1)
//                 finalScore += data.BonusPerFirstEnemyUnit;
//             if (enemiesCount > 1)
//                 finalScore += (enemiesCount - 1) * data.BonusPerNextEnemyUnits;
//
//             var pointsLeft = artillery.CurrentActionPoints;
//             foreach (var nextNode in path)
//             {
//                 if (artillery.Node == nextNode) continue;
//                 pointsLeft = artillery.MovePointsModifier.Modify(pointsLeft);
//             }
//
//             pointsLeft = artillery.AttackPointsModifier.Modify(pointsLeft);
//
//             finalScore += pointsLeft * data.BonusPerPoints;
//             return finalScore;
//         }
//     }
// }