using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public static class ComponentUnitExtension
    {
        public static bool IsBlocked([NotNull] this ComponentUnit unit)
        {
            if (unit == null) throw new ArgumentNullException(nameof(unit));
            return unit.TryGetExecutorAction<ComponentUnit.ContAttackAction>(out var action)
                   && action.IsBlocked;
        }
    }
}