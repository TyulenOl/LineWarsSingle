using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public partial class EnemyAI
    {
        public class BlockActionData : EnemyActionData
        {
            //[SerializeField] private 
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
            public BlockAction(EnemyAI enemy, IExecutor executor, BlockActionData data) : base(enemy, executor)
            {
                if(!(executor is Unit))
                    Debug.LogError("Executor isn't a unit!");
                if (!((Unit)executor).CanBlock())
                {
                    Debug.LogError("Invalid Action: Can't execute this command!");
                }

                this.data = data;
            }

            public override void Execute()
            {
                UnitsController.ExecuteCommand(new EnableBlockCommand((Unit) Executor), false);
            }

            protected override float GetScore()
            {
                return 1f;
            }
        }
    }
}
