using UnityEngine;
using System.Collections.Generic;

namespace LineWars.Model
{
    public abstract class UnitCost : ScriptableObject
    {
        public abstract PurchaseInfo Calculate(int baseCost, int unitCount);

        public IEnumerable<PurchaseInfo> CalculateMultiple(int baseCost, int minUnitCount, int maxUnitCount)
        {
            for(int unitCount = maxUnitCount;  unitCount <= maxUnitCount; unitCount++)
                yield return Calculate(baseCost, unitCount);
        }

        public IncrementInfo GetFirstIncrement(int baseCost)
        {
            var info1 = Calculate(baseCost, 0);
            var info2 = Calculate(baseCost, 1);
            return new IncrementInfo(info2 - info1, info2.CanBuy);
        }
    }

    public class IncrementInfo
    {
        public readonly int Cost;
        public readonly bool CanBuy;

        public IncrementInfo(int cost, bool canBuy)
        {
            Cost = cost;
            CanBuy = canBuy;
        }
    }
}
