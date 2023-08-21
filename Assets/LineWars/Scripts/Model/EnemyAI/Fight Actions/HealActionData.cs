using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace LineWars.Model
{
    public partial class EnemyAI
    {
        public class HealActionData : EnemyActionData
        {
            public override void AddAllPossibleActions(List<EnemyAction> list, EnemyAI enemy, IExecutor executor)
            {
                if(!(executor is Doctor doctor)) return;
                
                foreach(var owned in enemy.OwnedObjects)
                {
                    if(!(owned is Unit unit)) continue;
                    if(unit.CurrentHp == unit.MaxHp) continue;
                    var pathToNode = Graph.FindShortestPath(doctor.Node, unit.Node, doctor.Passability);
                    if(!pathToNode.Remove(unit.Node)) continue;
                    var healCost = doctor.CurrentActionPoints -
                                   doctor.HealPointModifier.Modify(doctor.CurrentActionPoints);
                    var moveCost = doctor.CurrentActionPoints -
                                   doctor.MovePointsModifier.Modify(doctor.CurrentActionPoints);
                    
                    if(moveCost + healCost > doctor.CurrentActionPoints) continue;
                    list.Add(new HealAction(enemy, executor, unit));
                }
            }
        }

        public class HealAction : EnemyAction
        {
            private readonly Unit damageUnit;
            private readonly Doctor doctor;
            public HealAction(EnemyAI enemy, IExecutor executor, Unit damagedUnit) 
                : base(enemy, executor)
            {
                if (!(executor is Doctor doctor))
                {
                    Debug.LogError("Executor is not a Doctor!");
                    return;
                }

                this.doctor = doctor;
                
                
            }

            public override void Execute()
            {
                throw new System.NotImplementedException();
            }

            protected override float GetScore()
            {
                throw new System.NotImplementedException();
            }
        }
    }

}