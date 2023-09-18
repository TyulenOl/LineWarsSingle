﻿using System;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New UnitHealActionData", menuName = "UnitActions/HealAction", order = 61)]
    public class UnitHealActionData: BaseUnitActionData
    {
        [field: SerializeField] public bool IsMassHeal { get; private set; }
        [field: SerializeField, Min(0)] public int HealingAmount { get; private set; }
        [field: SerializeField] public bool HealLocked { get; private set; }
        public override ComponentUnit.UnitAction GetAction(ComponentUnit unit) => new ComponentUnit.HealAction(unit, this);
    }
    
    public sealed partial class ComponentUnit
    {
        public class HealAction: UnitAction
        {
            public bool IsMassHeal { get; private set; }
            public int HealingAmount { get; private set; }
            public bool HealLocked { get; private set; }
            
            
            public HealAction([NotNull] ComponentUnit unit, UnitHealActionData data) : base(unit, data)
            {
                IsMassHeal = data.IsMassHeal;
                HealingAmount = data.HealingAmount;
                HealLocked = data.HealLocked;
            }

            public override CommandType GetMyCommandType() => CommandType.Heal;
            
            public bool CanHeal([NotNull] ComponentUnit target, bool ignoreActionPointsCondition = false)
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

            public void Heal([NotNull] ComponentUnit target)
            {
                target.HealMe(HealingAmount);
                if (IsMassHeal && MyUnit.TryGetNeighbour(out var neighbour))
                    neighbour.HealMe(HealingAmount);
                SfxManager.Instance.Play(ActionSfx);
                
                CompleteAndAutoModify();
            }
        }
    }
}