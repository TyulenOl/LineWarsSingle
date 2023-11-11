using System;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoHealAction : MonoUnitAction<HealAction<Node, Edge, Unit>>,
        IHealAction<Node, Edge, Unit>
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
        
        protected override HealAction<Node, Edge, Unit> GetAction()
        {
            var action = new HealAction<Node, Edge, Unit>(
                Unit,
                InitialIsMassHeal,
                InitialHealingAmount);
            return action;
        }
        
        public override void Accept(IMonoUnitActionVisitor visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor) => visitor.Visit(this);
    }
}