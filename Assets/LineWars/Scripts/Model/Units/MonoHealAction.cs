using UnityEngine;

namespace LineWars.Model
{
    public class MonoHealAction: MonoUnitAction
    {
        [field: SerializeField] public bool IsMassHeal { get; private set; }
        [field: SerializeField, Min(0)] public int HealingAmount { get; private set; }
        protected override UnitAction GetAction(ComponentUnit unit) => new HealAction(unit, this);
    }
}