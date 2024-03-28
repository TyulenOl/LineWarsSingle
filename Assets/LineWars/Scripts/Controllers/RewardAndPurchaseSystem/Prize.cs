using System;
using UnityEngine;

namespace LineWars.Model
{
    [System.Serializable]
    public class Prize
    {
        [SerializeField] private PrizeType type;
        [SerializeField] private int amount;

        public PrizeType Type => type;
        public int Amount => amount;

        public Prize(PrizeType type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }

        public override string ToString()
        {
            var currency = type switch
            {
                PrizeType.Gold => amount.Pluralize("монета", "монеты", "монет"),
                PrizeType.Diamonds => amount.Pluralize("кристалл", "кристалла", "кристаллов"),
                PrizeType.UpgradeCards => amount.Pluralize("карта улучшений", "карты улучшений", "карт улучшений"),
                _ => throw new ArgumentOutOfRangeException()
            };

            return $"{amount} {currency}";
        }
    }
}