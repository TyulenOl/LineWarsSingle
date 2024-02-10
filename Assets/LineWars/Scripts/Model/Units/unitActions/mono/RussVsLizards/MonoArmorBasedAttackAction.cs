using UnityEngine;

namespace LineWars.Model
{
    public class MonoArmorBasedAttackAction :
        MonoUnitAction<ArmorBasedAttackAction<Node, Edge, Unit>>,
        ITargetedAction<Unit>,
        IArmorBasedAttackAction<Node, Edge, Unit>
    {
        [SerializeField] private ActionUnitAnimation attackAnimation;
        protected override bool NeedAutoComplete => false;
        protected override ArmorBasedAttackAction<Node, Edge, Unit> GetAction()
        {
            return new ArmorBasedAttackAction<Node, Edge, Unit>(Executor);
        }

        public void Execute(Unit target)
        {
            if (attackAnimation != null)
                AnimationAttack(target);
            else
                AttackInstant(target);
        }

        private void AttackInstant(Unit target)
        {
            Action.Execute(target);
            Complete();
        }

        private void AnimationAttack(Unit target)
        {
            var context = new AnimationContext()
            {
                TargetUnit = target
            };
            attackAnimation.SetAction(OnAttack);
            attackAnimation.Ended.AddListener(OnAnimationEnd);
            attackAnimation.Execute(context);

            void OnAnimationEnd(UnitAnimation animation)
            {
                attackAnimation.Ended.RemoveListener(OnAnimationEnd);
                Complete();
            }

            void OnAttack()
            {
                Action.Execute(target);
                if (target.TryGetComponent<AnimationResponses>(out var responses))
                {
                    RespondToMeleeDamage(responses);
                }
            }
        }

        private void RespondToMeleeDamage(AnimationResponses unitResponses)
        {
            if (unitResponses != null)
            {
                var respondContext = new AnimationContext()
                {
                    TargetUnit = Executor
                };
                unitResponses.Respond(AnimationResponseType.MeleeDamaged, respondContext);
            }
        }

        public bool IsAvailable(Unit target)
        {
            return Action.IsAvailable(target);
        }

        public IActionCommand GenerateCommand(Unit target)
        {
            return new TargetedUniversalCommand
                <Unit, MonoArmorBasedAttackAction, Unit>(Executor, target);
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
