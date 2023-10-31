using System;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoHealAction : MonoUnitAction<HealAction<Node, Edge, Unit, Owned, BasePlayer>>,
        IHealAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        [SerializeField] private SFXData healSfx;
        [field: SerializeField] public bool InitialIsMassHeal { get; private set; }
        [field: SerializeField, Min(0)] public int InitialHealingAmount { get; private set; }

        public bool IsMassHeal => Action.IsMassHeal;
        public int HealingAmount => Action.HealingAmount;

        public bool CanHeal(Unit target, bool ignoreActionPointsCondition = false) =>
            Action.CanHeal(target, ignoreActionPointsCondition);

        public void Heal(Unit target)
        {
            Action.Heal(target);
            SfxManager.Instance.Play(healSfx);
        }

        public Type TargetType => typeof(Unit);
        public bool IsMyTarget(ITarget target) => target is Unit;

        public ICommandWithCommandType GenerateCommand(ITarget target)
        {
            return new HealCommand<Node, Edge, Unit, Owned, BasePlayer>(this, (Unit) target);
        }

        public override void Accept(IMonoUnitVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override HealAction<Node, Edge, Unit, Owned, BasePlayer> GetAction()
        {
            var action = new HealAction<Node, Edge, Unit, Owned, BasePlayer>(
                Unit,
                InitialIsMassHeal,
                InitialHealingAmount);
            return action;
        }
    }
}