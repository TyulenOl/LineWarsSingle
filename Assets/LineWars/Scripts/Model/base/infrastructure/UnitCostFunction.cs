using UnityEngine;

namespace LineWars.Model
{
    public abstract class UnitCostFunction : ScriptableObject
    {
        public abstract PurchaseInfo Calculate(UnitType unitType, int baseCost, int unitCount);
    }
}