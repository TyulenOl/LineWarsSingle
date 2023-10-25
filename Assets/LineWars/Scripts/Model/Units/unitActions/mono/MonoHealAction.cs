using System;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoHealAction: MonoUnitAction, IHealAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        [SerializeField] private SFXData healSfx;
        private HealAction<Node, Edge, Unit, Owned, BasePlayer> HealAction
            => (HealAction<Node, Edge, Unit, Owned, BasePlayer>) ExecutorAction;
        [field: SerializeField] public bool InitialIsMassHeal { get; private set; }
        [field: SerializeField, Min(0)] public int InitialHealingAmount { get; private set; }

        public bool IsMassHeal => HealAction.IsMassHeal;
        public int HealingAmount => HealAction.HealingAmount;
        
        public bool CanHeal(Unit target, bool ignoreActionPointsCondition = false) => 
            HealAction.CanHeal(target, ignoreActionPointsCondition);

        public void Heal(Unit target)
        {
            HealAction.Heal(target);
            SfxManager.Instance.Play(healSfx);
        }

        public Type TargetType => typeof(Unit);
        public bool IsMyTarget(ITarget target) => target is Unit;
        public ICommandWithCommandType GenerateCommand(ITarget target)
        {
            return new HealCommand<Node, Edge, Unit, Owned, BasePlayer>(this, (Unit) target);
        }

        protected override ExecutorAction GetAction()
        {
            var action = new HealAction<Node, Edge, Unit, Owned, BasePlayer>(Unit, this);
            return action;
        }

        public override void Accept(IMonoUnitVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}