using System;

namespace LineWars.Model
{
    public enum PrizeType
    {
        Gold,
        Diamonds,
        UpgradeCards
    }

    public static class PrizeTypeHelper
    {
        public static CostType ToCostType(this PrizeType prizeType)
        {
            return prizeType switch
            {
                PrizeType.Gold => CostType.Gold,
                PrizeType.Diamonds => CostType.Diamond,
                _ => throw new ArgumentOutOfRangeException(nameof(prizeType), prizeType, null)
            };
        }
    }
}