// using System.Linq;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace LineWars.Model
// {
//     [CreateAssetMenu(fileName = "New Move To Own Node", menuName = "EnemyAI/Enemy Actions/Fight Phase/Move To Own Node")]
//     public class EnemyAIMoveToOwnNodeActionData : EnemyActionData
//     {
//         [SerializeField] private float waitTime;
//         [SerializeField] private float baseScore;
//         [SerializeField] private float bonusPerUnitHp;
//         [SerializeField] private float penaltyForEnemiesAttack;
//         [SerializeField] private float bonusPerDistance;
//         [SerializeField] private float bonusPerPoint;
//
//         public float BaseScore => baseScore;
//         public float WaitTime => waitTime;
//         public float BonusPerUnitHp => bonusPerUnitHp;
//         public float PenaltyForEnemiesAttack => penaltyForEnemiesAttack;
//         public float BonusPerDistance => bonusPerDistance;
//         public float BonusPerPoint => bonusPerPoint;
//
//         public override void AddAllPossibleActions(List<EnemyAction> list, EnemyAI basePlayer, IExecutor executor)
//         {
//             if (!(executor is ComponentUnit unit)) return;
//
//             EnemyActionUtilities.GetNodesInIntModifierRange(unit.Node, unit.CurrentActionPoints,
//                 unit.MovePointsModifier,
//                 (prevNode, node, actionPoints) => NodeParser(prevNode, node, actionPoints, basePlayer, unit, list),
//                 unit);
//         }
//
//         private void NodeParser(Node _, Node node, int actionPoints, EnemyAI basePlayer, ComponentUnit unit,
//             List<EnemyAction> actionList)
//         {
//             if (actionPoints < 0) return;
//             if (node == unit.Node) return;
//             if (node.Owner == basePlayer)
//                 actionList.Add(new MoveToOwnNodeAction(basePlayer, unit, node, this));
//         }
//     }
//
//     public class MoveToOwnNodeAction : EnemyAction
//     {
//         private readonly Node targetNode;
//         private readonly ComponentUnit unit;
//         private readonly List<Node> path;
//         private readonly EnemyAIMoveToOwnNodeActionData data;
//
//         public MoveToOwnNodeAction(EnemyAI enemy, IExecutor executor,
//             Node targetNode, EnemyAIMoveToOwnNodeActionData data) : base(enemy, executor)
//         {
//             this.targetNode = targetNode;
//             if (executor is not ComponentUnit unit)
//             {
//                 Debug.LogError("Executor is not a Unit");
//                 return;
//             }
//
//             this.unit = unit;
//             this.data = data;
//             path = Graph.FindShortestPath(unit.Node, targetNode, unit);
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
//                     if (node == unit.Node) continue;
//                     if (!unit.CanMoveTo(node))
//                         Debug.LogError($"Unit cannot move to {node}");
//
//                     UnitsController.ExecuteCommand(new UnitMoveCommand(unit, node));
//                     yield return new WaitForSeconds(data.WaitTime);
//                 }
//
//                 InvokeActionCompleted();
//             }
//         }
//
//         protected float GetScore()
//         {
//             var enemies = EnemyActionUtilities.FindAdjacentEnemies(unit.Node, basePlayer);
//             var enemiesAttack = 0;
//             foreach (var enemy in enemies)
//             {
//                 enemiesAttack += enemy.Damage;
//             }
//
//             var safetyBonus = data.BonusPerUnitHp * unit.CurrentHp
//                               - data.PenaltyForEnemiesAttack * enemiesAttack;
//             var oldNodeDistance = Graph.FindShortestPath(unit.Node, basePlayer.Base.Node).Count;
//             var newNodeDistance = Graph.FindShortestPath(targetNode, basePlayer.Base.Node).Count;
//             var pointsLeft = unit.CurrentActionPoints;
//             foreach (var node in path)
//             {
//                 if (node == unit.Node) continue;
//                 pointsLeft = unit.MovePointsModifier.Modify(pointsLeft);
//             }
//
//             return data.BaseScore
//                    + safetyBonus
//                    + data.BonusPerDistance * (newNodeDistance - oldNodeDistance)
//                    + data.BonusPerPoint * pointsLeft;
//         }
//     }
// }