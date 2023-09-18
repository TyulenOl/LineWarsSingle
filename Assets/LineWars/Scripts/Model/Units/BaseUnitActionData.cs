using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public abstract class BaseUnitActionData: BaseExecutorActionData
    {
        public override ExecutorAction GetAction(IExecutor executor)
        {
            if (executor is not ComponentUnit unit)
                throw new ArgumentException($"{nameof(executor)} is not {nameof(ComponentUnit)}!");
            return GetAction(unit);
        }
        public abstract ComponentUnit.UnitAction GetAction(ComponentUnit unit);
    }

    public sealed partial class ComponentUnit
    {
        public abstract class UnitAction: ExecutorAction
        {
            protected readonly ComponentUnit MyUnit;
            protected UnitAction([NotNull] ComponentUnit unit, BaseUnitActionData data) : base(unit, data)
            {
                MyUnit = unit;
            }
            
            public virtual uint GetPossibleMaxRadius() => (uint) MyUnit.CurrentActionPoints;
            
            protected void CompleteAndAutoModify()
            {
                MyUnit.CurrentActionPoints = ModifyActionPoints();
                Complete();
            }
        }
    }

}