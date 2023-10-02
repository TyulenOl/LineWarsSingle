﻿using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoHealAction: MonoUnitAction, IHealAction<Node, Edge, Unit, Owned, BasePlayer, Nation>
    {
        [SerializeField] private SFXData healSfx;
        private HealAction<Node, Edge, Unit, Owned, BasePlayer, Nation> HealAction
            => (HealAction<Node, Edge, Unit, Owned, BasePlayer, Nation>) ExecutorAction;
        [field: SerializeField] public bool InitialIsMassHeal { get; private set; }
        [field: SerializeField, Min(0)] public int InitialHealingAmount { get; private set; }

        public bool IsMassHeal => HealAction.IsMassHeal;
        public int HealingAmount => HealAction.HealingAmount;
        public bool HealLocked => HealAction.HealLocked;
        public bool CanHeal(Unit target, bool ignoreActionPointsCondition = false) => 
            HealAction.CanHeal(target, ignoreActionPointsCondition);

        public void Heal(Unit target)
        {
            HealAction.Heal(target);
            SfxManager.Instance.Play(healSfx);
        }
        
        public bool IsMyTarget(ITarget target) => HealAction.IsMyTarget(target);
        public ICommand GenerateCommand(ITarget target)
        {
            return new HealCommand<Node, Edge, Unit, Owned, BasePlayer, Nation>(this, (Unit) target);
        }

        protected override ExecutorAction GetAction()
        {
            var action = new HealAction<Node, Edge, Unit, Owned, BasePlayer, Nation>(Unit, this);
            return action;
        }
    }
}