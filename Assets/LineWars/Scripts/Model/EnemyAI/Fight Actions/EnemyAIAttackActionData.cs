// using System;
// using System.Collections;
// using System.Collections.Generic;
// using JetBrains.Annotations;
// using UnityEngine;
//
// namespace LineWars.Model
// {
//     [CreateAssetMenu(fileName = "New Attack Action", menuName = "EnemyAI/Enemy Actions/Fight Phase/Attack")]
//     public class EnemyAIAttackActionData : EnemyActionData
//     {
//         [SerializeField] private float waitTime;
//         [SerializeField] private float baseScore;
//         [SerializeField] private float bonusPerDistance;
//         [SerializeField] private float bonusPerEnemyHpDamage;
//         [SerializeField] private float bonusPerPoint;
//
//         public float BaseScore => baseScore;
//         public float WaitTime => waitTime;
//         public float BonusPerDistance => bonusPerDistance;
//         public float BonusPerEnemyHpDamage => bonusPerEnemyHpDamage;
//         public float BonusPerPoint => bonusPerPoint;
//
//         public override void AddAllPossibleActions(List<EnemyAction> list, EnemyAI basePlayer, IExecutor executor)
//         {
//             if (executor is not ComponentUnit unit) return;
//             var enemySet = new HashSet<ComponentUnit>();
//
//             EnemyActionUtilities.GetNodesInIntModifierRange(unit.Node, unit.CurrentActionPoints,
//                 unit.MovePointsModifier,
//                 (prevNode, node, range) =>
//                     NodeParser(prevNode, node, range, enemySet, unit, basePlayer, list), unit);
//         }
//
//         private void NodeParser(Node previousNode, Node node, int actionPoints,
//             HashSet<ComponentUnit> enemySet, ComponentUnit unit, EnemyAI basePlayer, List<EnemyAction> actionList)
//         {
//             if (actionPoints <= 0) return;
//             var pointsAfterAttack = unit.AttackPointsModifier.Modify(actionPoints);
//             if (pointsAfterAttack < 0) return;
//
//             var enemies = EnemyActionUtilities.FindAdjacentEnemies(node, basePlayer, LineType.Firing);
//             foreach (var enemy in enemies)
//             {
//                 if (enemySet.Contains(enemy)) continue;
//                 actionList.Add(new AttackAction(basePlayer, unit, enemy, this));
//                 enemySet.Add(enemy);
//             }
//         }
//     }
//
//     public class AttackAction : EnemyAction
//     {
//         private readonly ComponentUnit target;
//         private readonly ComponentUnit unit;
//         private readonly EnemyAIAttackActionData data;
//         private readonly List<Node> path;
//
//         public AttackAction([NotNull] EnemyAI basePlayer, [NotNull] IExecutor executor, [NotNull] ComponentUnit target,
//             [NotNull] EnemyAIAttackActionData data)
//             : base(basePlayer, executor)
//         {
//             if (basePlayer == null) throw new ArgumentNullException(nameof(basePlayer));
//             if (executor == null) throw new ArgumentNullException(nameof(executor));
//             if (target == null) throw new ArgumentNullException(nameof(target));
//             if (data == null) throw new ArgumentNullException(nameof(data));
//             if (executor is not ComponentUnit unit)
//             {
//                 Debug.LogError("Executor is not a Unit");
//                 return;
//             }
//
//             this.unit = unit;
//             this.target = target;
//             this.data = data;
//
//             path = Graph.FindShortestPath(unit.Node, target.Node, unit);
//             path.Remove(unit.Node);
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
//                     if (node == target.Node) continue;
//                     if (node == unit.Node) continue;
//                     if (!unit.CanMoveTo(node))
//                         Debug.LogError($"{unit} cannot move to {node}");
//
//                     UnitsController.ExecuteCommand(new UnitMoveCommand(unit, node));
//                     yield return new WaitForSeconds(data.WaitTime);
//                 }
//
//                 if (!unit.CanAttack(target))
//                     Debug.LogError($"{unit} cannot attack {target}");
//
//                 UnitsController.ExecuteCommand(new UnitAttackCommand(unit, target));
//                 InvokeActionCompleted();
//             }
//         }
//
//         private float GetScore()
//         {
//             if (unit.Damage == 0) return 0f;
//
//             var enemyDistance = Graph.FindShortestPath(target.Node, basePlayer.Base.Node, target).Count;
//             if (enemyDistance == 0) enemyDistance = 1;
//             var distanceBonus = data.BonusPerDistance / enemyDistance;
//             var hpBonus = data.BonusPerEnemyHpDamage
//                           * (1 - target.CurrentHp / target.MaxHp);
//             var pointsLeft = unit.CurrentActionPoints;
//             foreach (var node in path)
//             {
//                 if (node == target.Node) continue;
//                 if (node == unit.Node) continue;
//                 pointsLeft = unit.MovePointsModifier.Modify(pointsLeft);
//             }
//
//             pointsLeft = unit.AttackPointsModifier.Modify(pointsLeft);
//             return data.BaseScore
//                    + distanceBonus
//                    + hpBonus
//                    + pointsLeft * data.BonusPerPoint;
//         }
//     }
// }