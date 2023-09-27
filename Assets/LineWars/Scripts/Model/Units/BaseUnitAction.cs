using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public abstract class BaseUnitAction: BaseExecutorAction
    {
        public override ExecutorAction GetAction(IReadOnlyExecutor executor)
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
            public readonly ComponentUnit MyUnit;
            protected UnitAction([NotNull] ComponentUnit unit, BaseUnitAction data) : base(unit, data)
            {
                MyUnit = unit;
            }
            
            public virtual uint GetPossibleMaxRadius() => (uint) MyUnit.CurrentActionPoints;
        }
    }

}