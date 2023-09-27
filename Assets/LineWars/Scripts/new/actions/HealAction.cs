using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;


namespace LineWars.Model
{
    public class HealAction : UnitAction, ITargetedAction
    {
        public bool IsMassHeal { get; private set; }
        public int HealingAmount { get; private set; }
        public bool HealLocked { get; private set; }
        public HealAction([NotNull] IUnit unit, MonoHealAction data) : base(unit, data)
        {
            IsMassHeal = data.IsMassHeal;
            HealingAmount = data.HealingAmount;
        }

        public ICommand GenerateCommand(ITarget target)
        {
            throw new NotImplementedException();
        }
        public bool CanHeal([NotNull] IReadOnlyUnit target, bool ignoreActionPointsCondition = false)
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

        public void Heal([NotNull] IUnit target)
        {
            target.HealMe(HealingAmount);
            if (IsMassHeal && MyUnit.TryGetNeighbour(out var neighbour))
            neighbour.HealMe(HealingAmount);
          
                     
            CompleteAndAutoModify();
        }

        public override CommandType GetMyCommandType() => CommandType.Heal;

        public bool IsMyTarget(ITarget target) => target is IUnit;
        public ICommand GenerateCommand(IReadOnlyTarget target) => new HealCommand(this, (IUnit)target);
    }
}
