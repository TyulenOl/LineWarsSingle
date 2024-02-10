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
    }
}
