using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public static class ComponentUnitExtension
    {
        public static bool GetIsBlocked([NotNull] this ModelComponentUnit unit)
        {
            if (unit == null) throw new ArgumentNullException(nameof(unit));
            return unit.TryGetExecutorAction<ModelComponentUnit.BlockAction>(out var action)
                   && action.IsBlocked;
        }

        public static int GetDamage([NotNull] this ModelComponentUnit unit)
        {
            if (unit == null) throw new ArgumentNullException(nameof(unit));
            return unit.TryGetExecutorAction<ModelComponentUnit.BaseAttackAction>(out var action)
                ? action.Damage
                : 0;
        }
    }
}