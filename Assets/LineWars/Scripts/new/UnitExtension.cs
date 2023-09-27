using System;
using System.Diagnostics.CodeAnalysis;
using LineWars.Model;

namespace LineWars.Scripts
{
    public static class UnitExtension
    {
        public static bool GetIsBlocked([NotNull] this IReadOnlyUnit unit)
        {
            if (unit == null) throw new ArgumentNullException(nameof(unit));
            return unit.TryGetExecutorAction<ComponentUnit.BlockAction>(out var action)
                   && action.IsBlocked;
        }
    }
}