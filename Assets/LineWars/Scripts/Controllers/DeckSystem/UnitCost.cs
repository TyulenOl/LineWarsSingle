using UnityEngine;

namespace LineWars.Model
{
    public abstract class UnitCost : ScriptableObject
    {
        public abstract PurchaseInfo Calculate(int unitCost, int unitCount);
    }
}
