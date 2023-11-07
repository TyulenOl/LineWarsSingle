using System;
using System.Collections.Generic;

namespace LineWars
{
    public enum PhaseType
    {
        Idle,
        Buy,
        Artillery,
        Fight,
        Scout,
        Replenish
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
                {PhaseType.Idle, PhaseMode.NotPlayable},
                {PhaseType.Buy, PhaseMode.Simultaneous},
                {PhaseType.Artillery, PhaseMode.Alternating},
                {PhaseType.Fight,PhaseMode.Alternating},
                {PhaseType.Scout, PhaseMode.Alternating},
                {PhaseType.Replenish, PhaseMode.NotPlayable}
            };

        public static PhaseType Next(PhaseType type, PhaseOrderData orderData)
        {
            if (type == PhaseType.Idle) throw new ArgumentException("Idle Phase don't have next Phases!"); 
            var index = orderData.FindIndex(type);
            if (index == -1) throw new ArgumentException("Order Data doesn't contains given PhaseType!");

            var newIndex = (index + 1) % orderData.Count;
            return orderData[newIndex];
        }
    }
}
