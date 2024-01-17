using UnityEngine;

namespace LineWars.Model
{
    public class MonoHealingAttackAction :
        MonoUnitAction<HealingAttackAction<Node, Edge, Unit>>,
        ITargetedAction<Unit>,
        IHealingAttackAction<Node, Edge, Unit>
    {
        [SerializeField] private ActionUnitAnimation attackAnimation;

        protected override bool NeedAutoComplete => false;
        protected override HealingAttackAction<Node, Edge, Unit> GetAction()
        {
            return new HealingAttackAction<Node, Edge, Unit>(Executor);
        }

        public bool IsAvailable(Unit target)
        {
            return Action.IsAvailable(target);
        }

        public void Execute(Unit target)
        {
            if (attackAnimation == null)
                AttackInstant(target);
            else
                AnimationAttack(target);
        }

        private void AttackInstant(Unit target)
        {
            Action.Execute(target);
            Player.LocalPlayer.RecalculateVisibility();
            Complete();
        }

        private void AnimationAttack(Unit target)
        {
            var context = new AnimationContext()
            {
                TargetUnit = target
            };
            attackAnimation.SetAction(() => Action.Execute(target));
            attackAnimation.Ended.AddListener(OnAnimationEnd);
            attackAnimation.Execute(context);

            void OnAnimationEnd(UnitAnimation _)
            {
                attackAnimation.Ended.RemoveListener(OnAnimationEnd);
                Player.LocalPlayer.RecalculateVisibility();
                Complete();
            }
        }

        public IActionCommand GenerateCommand(Unit target)
        {
            return new TargetedUniversalCommand<Unit, MonoHealingAttackAction, Unit>
                (this, target);
        }

        public override void Accept(IMonoUnitActionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
