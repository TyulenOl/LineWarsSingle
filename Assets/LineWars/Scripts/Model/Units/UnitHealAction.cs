﻿using System;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    //[CreateAssetMenu(fileName = "New UnitHealActionData", menuName = "UnitActions/HealAction", order = 61)]
    public class UnitHealAction: BaseUnitAction
    {
        [field: SerializeField] public bool IsMassHeal { get; private set; }
        [field: SerializeField, Min(0)] public int HealingAmount { get; private set; }
        public override ModelComponentUnit.UnitAction GetAction(ModelComponentUnit unit) => new ModelComponentUnit.HealAction(unit, this);
    }
    
    public sealed partial class ModelComponentUnit
    {
        public class HealAction: UnitAction, ITargetedAction
        {
            public bool IsMassHeal { get; private set; }
            public int HealingAmount { get; private set; }
            public bool HealLocked { get; private set; }
            
            
            public HealAction([NotNull] ModelComponentUnit unit, UnitHealAction data) : base(unit, data)
            {
                IsMassHeal = data.IsMassHeal;
                HealingAmount = data.HealingAmount;
            }

            public override CommandType GetMyCommandType() => CommandType.Heal;
            
            public bool CanHeal([NotNull] ModelComponentUnit target, bool ignoreActionPointsCondition = false)
            {
                return !HealLocked 
                       && OwnerCondition()
                       && SpaceCondition() 
                       && (ignoreActionPointsCondition || ActionPointsCondition())
                       && target != MyUnit 
                       && target.CurrentHp != target.MaxHp;

                bool SpaceCondition()
                {
                    var line = MyUnit.Node.GetLine(target.Node);
                    return line != null || MyUnit.IsNeighbour(target);
                }

                bool OwnerCondition()
                {
                    return target.Owner == MyUnit.Owner;
                }
            }

            public void Heal([NotNull] ModelComponentUnit target)
            {
                target.HealMe(HealingAmount);
                if (IsMassHeal && MyUnit.TryGetNeighbour(out var neighbour))
                    neighbour.HealMe(HealingAmount);
                
                CompleteAndAutoModify();
            }

            public bool IsMyTarget(ITarget target) => target is ModelComponentUnit;
            public ICommand GenerateCommand(ITarget target) => new UnitHealCommand(this, (ModelComponentUnit) target);
        }
    }
}