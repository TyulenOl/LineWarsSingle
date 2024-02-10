using System;
using System.Collections.Generic;

namespace LineWars
{
    public enum PhaseType
    {
        Buy,
        Artillery,
        Fight,
        Scout,
        Replenish,
        None,
        Payday
    }

    public enum PhaseMode
    {
        Alternating,
        Simultaneous,
        NotPlayable
    }

    public static class PhaseHelper
    {
        public static Dictionary<PhaseType, PhaseMode> TypeToMode
            = new Dictionary<PhaseType, PhaseMode>()
            {
                {PhaseType.Buy, PhaseMode.Simultaneous},
                {PhaseType.Artillery, PhaseMode.Alternating},
                {PhaseType.Fight,PhaseMode.Alternating},
                {PhaseType.Scout, PhaseMode.Alternating},
                {PhaseType.Replenish, PhaseMode.NotPlayable},
                {PhaseType.Payday, PhaseMode.Simultaneous}
            };

        public static PhaseType Next(this PhaseType type, IReadOnlyList<PhaseType> orderData)
        {
            var index = orderData.FindIndex(type);
            if (index == -1) throw new ArgumentException("Order Data doesn't contains given PhaseType!");

            var newIndex = (index + 1) % orderData.Count;
            return orderData[newIndex];
        }

        public static bool IsAnyFightPhase(this PhaseType phaseType)
        {
            return phaseType is PhaseType.Artillery or PhaseType.Scout or PhaseType.Fight;
        }
    }
}
