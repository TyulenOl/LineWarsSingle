using System;
using JetBrains.Annotations;
using LineWars.Controllers;

namespace LineWars.Model
{
    public abstract class BaseUnitAction: BaseExecutorAction
    {
        public override ExecutorAction GetAction(IReadOnlyExecutor executor)
        {
            if (executor is not ComponentUnit unit)
                throw new ArgumentException($"{nameof(executor)} is not {nameof(ComponentUnit)}!");
            var action = GetAction(unit);
            return action;
        }
        public abstract UnitAction GetAction(ComponentUnit unit);
    }
}