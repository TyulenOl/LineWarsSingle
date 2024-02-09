using System;
using UnityEngine;

namespace LineWars.Model
{
    [Obsolete]
    public abstract class UnitCostFunction : ScriptableObject
    {
        public abstract PurchaseInfo Calculate(UnitType unitType, int baseCost, int unitCount);
    }
}