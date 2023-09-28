using UnityEngine;

namespace LineWars.Model
{
    [RequireComponent(typeof(MonoAttackAction))]
    public class MonoBlockAction : MonoUnitAction
    {
        [SerializeField] private bool protection = false;
        [SerializeField] private IntModifier contrAttackDamageModifier;
        public bool Protection => protection;
        public IntModifier ContrAttackDamageModifier => contrAttackDamageModifier;

        protected override UnitAction GetAction(ComponentUnit unit) =>
            new BlockAction(unit, this);
    }
}