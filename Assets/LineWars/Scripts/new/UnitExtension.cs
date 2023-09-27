using System;
using System.Diagnostics.CodeAnalysis;
using LineWars.Model;

namespace LineWars
{
    public static class UnitExtension
    {
        public static bool GetIsBlocked([NotNull] this IReadOnlyUnit unit)
        {
            if (unit == null) throw new ArgumentNullException(nameof(unit));
            return unit.TryGetExecutorAction<BlockAction>(out var action)
                   && action.IsBlocked;
        }

        public static int GetDamage([NotNull] this IReadOnlyUnit unit)
        {
            if (unit == null) throw new ArgumentNullException(nameof(unit));
            return unit.TryGetExecutorAction<AttackAction>(out var action) ? action.Damage : 0;
        }
    }
}