// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace LineWars.Model
// {
//     [CreateAssetMenu(fileName = "New Block Action Data", menuName = "EnemyAI/Enemy Actions/Block")]
//     public class EnemyAIBlockActionData : EnemyActionData
//     {
//         [SerializeField] private float baseScore;
//         [SerializeField] private float bonusPerPoint;
//         [SerializeField] private float bonusPerEnemy;
//         [SerializeField] private float bonusPerHp;
//         [SerializeField] private float bonusPerCost;
//         [SerializeField, Range(0, 1)] private float maxHpToAddCostBonus;
//
//         public float BaseScore => baseScore;
//         public float BonusPerPoint => bonusPerPoint;
//         public float BonusPerEnemy => bonusPerEnemy;
//         public float BonusPerHp => bonusPerHp;
//         public float BonusPerCost => bonusPerCost;
//         public float MaxHpToAddCostBonus => maxHpToAddCostBonus;
//
//         public override void AddAllPossibleActions(List<EnemyAction> list, EnemyAI basePlayer, IExecutor executor)
//         {
//             if (!(executor is ComponentUnit unit))
//                 return;
//             if (unit.CanBlock())
//                 list.Add(new BlockAction(basePlayer, executor, this));
//         }
//     }
//
//     public class BlockAction : EnemyAction
//     {
//         private readonly EnemyAIBlockActionData data;
//         private readonly ComponentUnit unit;
//
//         public BlockAction(EnemyAI basePlayer, IExecutor executor, EnemyAIBlockActionData data) : base(basePlayer,
//             executor)
//         {
//             if (!(executor is ComponentUnit unit))
//             {
//                 Debug.LogError("Executor isn't a unit!");
//                 return;
//             }
//
//             if (!((ComponentUnit) executor).CanBlock())
//             {
//                 Debug.LogError("Invalid Action: Can't execute this command!");
//             }
//
//             this.data = data;
//             this.unit = unit;
//             score = GetScore();
//         }
//
//         public override void Execute()
//         {
//             UnitsController.ExecuteCommand(new EnableBlockCommand((ComponentUnit) Executor), false);
//             InvokeActionCompleted();
//         }
//
//         private float GetScore()
//         {
//             var enemyCount = EnemyActionUtilities.FindAdjacentEnemies(unit.Node, basePlayer).Count;
//             var damagePercent = (float) unit.CurrentHp / unit.MaxHp;
//             var costBonus = damagePercent <= data.MaxHpToAddCostBonus ? unit.Cost * data.BonusPerCost : 0;
//             var pointsLeft = unit.BlockPointsModifier.Modify(unit.CurrentActionPoints);
//
//             return data.BaseScore
//                    + enemyCount * data.BonusPerEnemy
//                    + (1 - damagePercent) * data.BonusPerHp
//                    + costBonus
//                    + pointsLeft * data.BonusPerPoint;
//         }
//     }
// }