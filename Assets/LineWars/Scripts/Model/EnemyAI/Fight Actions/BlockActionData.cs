using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public partial class EnemyAI
    {
        [CreateAssetMenu(fileName = "New Block Action Data", menuName = "EnemyAI/Enemy Actions/Block")]
        public class BlockActionData : EnemyActionData
        {
            [SerializeField] private float baseScore;
            [SerializeField] private float bonusPerPoint;
            [SerializeField] private float bonusPerEnemy;
            [SerializeField] private float bonusPerHp;
            [SerializeField] private float bonusPerCost;
            [SerializeField, Range(0, 1)] private float maxHpToAddCostBonus;
            
            public float BaseScore => baseScore;
            public float BonusPerPoint => bonusPerPoint;
            public float BonusPerEnemy => bonusPerEnemy;
            public float BonusPerHp => bonusPerHp;
            public float BonusPerCost => bonusPerCost;
            public float MaxHpToAddCostBonus => maxHpToAddCostBonus;
            public override void AddAllPossibleActions(List<EnemyAction> list, EnemyAI enemy, IExecutor executor)
            {
                if (!(executor is Unit unit))
                    return;
                if(unit.CanBlock())
                    list.Add(new BlockAction(enemy, executor, this));
            }
        }

        public class BlockAction : EnemyAction
        {
            private readonly BlockActionData data;
            private readonly Unit unit;
            public BlockAction(EnemyAI enemy, IExecutor executor, BlockActionData data) : base(enemy, executor)
            {
                if(!(executor is Unit))
                    Debug.LogError("Executor isn't a unit!");
                if (!((Unit)executor).CanBlock())
                {
                    Debug.LogError("Invalid Action: Can't execute this command!");
                }

                this.data = data;
                unit = (Unit) executor;
            }

            public override void Execute()
            {
                UnitsController.ExecuteCommand(new EnableBlockCommand((Unit) Executor), false);
            }

            protected override float GetScore()
            {
                var enemyCount = EnemyActionUtilities.FindAdjacentEnemies(unit.Node, enemy).Count;
                var damagePercent = 1 - (unit.CurrentHp / unit.MaxHp);
                var costBonus = damagePercent <= data.MaxHpToAddCostBonus ? unit.Cost * data.BonusPerCost : 0;
                var pointsLeft = unit.BlockPointsModifier.Modify(unit.CurrentActionPoints);
                return data.BaseScore 
                       + enemyCount * data.BonusPerEnemy 
                       + damagePercent * data.BonusPerHp
                       + costBonus
                       + pointsLeft * data.BonusPerPoint;
            }
        }
    }
}
