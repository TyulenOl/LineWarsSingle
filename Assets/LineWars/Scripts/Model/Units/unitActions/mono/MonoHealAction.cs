using UnityEngine;

namespace LineWars.Model
{
    public class MonoHealAction: MonoUnitAction, IHealAction<Node, Edge, Unit, Owned, BasePlayer, Nation>
    {
        private HealAction<Node, Edge, Unit, Owned, BasePlayer, Nation> HealAction
            => (HealAction<Node, Edge, Unit, Owned, BasePlayer, Nation>) ExecutorAction;
        [field: SerializeField] public bool InitialIsMassHeal { get; private set; }
        [field: SerializeField, Min(0)] public int InitialHealingAmount { get; private set; }

        public bool IsMassHeal => HealAction.IsMassHeal;
        public int HealingAmount => HealAction.HealingAmount;
        public bool HealLocked => HealAction.HealLocked;
        public bool CanHeal(Unit target, bool ignoreActionPointsCondition = false)
        {
            return HealAction.CanHeal(target, ignoreActionPointsCondition);
        }

        public void Heal(Unit target)
        {
            HealAction.Heal(target);
        }
        
        protected override ExecutorAction GetAction()
        {
            var action = new HealAction<Node, Edge, Unit, Owned, BasePlayer, Nation>(GetComponent<Unit>(), this);
            return action;
        }
    }
}