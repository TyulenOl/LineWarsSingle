using System;
using JetBrains.Annotations;
using LineWars.Controllers;

namespace LineWars.Model
{
    public abstract class BaseUnitAction: BaseExecutorAction
    {
        public override ExecutorAction GetAction(IReadOnlyExecutor executor)
        {
            if (executor is not ModelComponentUnit unit)
                throw new ArgumentException($"{nameof(executor)} is not {nameof(ModelComponentUnit)}!");
            var action = GetAction(unit);
            action.ActionCompleted += () => SfxManager.Instance.Play(ActionSfx);
            return action;
        }
        public abstract ModelComponentUnit.UnitAction GetAction(ModelComponentUnit unit);
    }

    public sealed partial class ModelComponentUnit
    {
        public abstract class UnitAction: ExecutorAction
        {
            public readonly ModelComponentUnit MyUnit;
            protected UnitAction([NotNull] ModelComponentUnit unit, BaseUnitAction data) : base(unit, data)
            {
                MyUnit = unit;
            }
            
            public virtual uint GetPossibleMaxRadius() => (uint) MyUnit.CurrentActionPoints;
        }
    }

}