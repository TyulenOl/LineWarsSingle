using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoHealingAttackAction :
        MonoUnitAction<HealingAttackAction<Node, Edge, Unit>>,
        ITargetedAction<Unit>,
        IHealingAttackAction<Node, Edge, Unit>
    {
        [SerializeField] private ActionUnitAnimation attackAnimation;
        [SerializeField] private SFXData attackReaction;

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
            Executor.PlaySfx(attackReaction);
            Complete();
        }

        private void AnimationAttack(Unit target)
        {
            var context = new AnimationContext()
            {
                TargetUnit = target
            };
            var responses = target.GetComponent<AnimationResponses>();
            if (responses != null)
            {
                responses.TrySetDeathAnimation(AnimationResponseType.MeleeDied);
            };
            attackAnimation.SetAction(OnAttack);
            attackAnimation.Ended.AddListener(OnAnimationEnd);
            attackAnimation.Execute(context);

            void OnAnimationEnd(UnitAnimation _)
            {
                attackAnimation.Ended.RemoveListener(OnAnimationEnd);
                Player.LocalPlayer.RecalculateVisibility();
                if (responses != null)
                {
                    responses.SetDefaultDeathAnimation();
                };
                Complete();
            }
            void OnAttack()
            {
                Action.Execute(target);
                Executor.PlaySfx(attackReaction);
                var context2 = new AnimationContext()
                {
                    TargetUnit = Executor,
                };

                if(responses != null)
                {
                    responses.Respond(AnimationResponseType.MeleeDamaged, context);
                };
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
