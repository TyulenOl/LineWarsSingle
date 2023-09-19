using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public static class ComponentUnitExtension
    {
        public static bool GetIsBlocked([NotNull] this ComponentUnit unit)
        {
            if (unit == null) throw new ArgumentNullException(nameof(unit));
            return unit.TryGetExecutorAction<ComponentUnit.BlockAction>(out var action)
                   && action.IsBlocked;
        }

        public static int GetDamage([NotNull] this ComponentUnit unit)
        {
            if (unit == null) throw new ArgumentNullException(nameof(unit));
            return unit.TryGetExecutorAction<ComponentUnit.BaseAttackAction>(out var action)
                ? action.Damage
                : 0;
        }
    }
}