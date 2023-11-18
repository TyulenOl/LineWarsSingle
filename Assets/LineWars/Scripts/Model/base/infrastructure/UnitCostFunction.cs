using UnityEngine;

namespace LineWars.Model
{
    public abstract class UnitCostFunction : ScriptableObject
    {
        public abstract PurchaseInfo Calculate(UnitType unitType, int baseCost, int unitCount);
    }

    public class PurchaseInfo
    {
        public int Cost { get; }
        public bool CanBuy => Cost >= 0;
        public PurchaseInfo(int cost)
        {
            Cost = cost;
        }
    }
}