using UnityEngine;

namespace LineWars.Model
{
    public class MonoTargetPowerBasedAttackAction :
        MonoUnitAction<TargetPowerBasedAttackAction<Node, Edge, Unit>>,
        ITargetedAction<Unit>,
        ITargetPowerBasedAttackAction<Node, Edge, Unit>
    {
        [SerializeField] private UnitAnimation attackAnimation;

        protected override bool NeedAutoComplete => false;
        protected override TargetPowerBasedAttackAction<Node, Edge, Unit> GetAction()
        {
            return new TargetPowerBasedAttackAction<Node, Edge, Unit>(Executor);
        }

        public bool IsAvailable(Unit target)
        {
            return Action.IsAvailable(target);
        }

        public void Execute(Unit target)
        {
            if(attackAnimation == null)
                AttackInstant(target);
            else
                AnimationAttack(target);
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

            attackAnimation.Ended.AddListener(OnAnimationEnd);
            attackAnimation.Execute(context);

            void OnAnimationEnd(UnitAnimation _)
            {
                attackAnimation.Ended.RemoveListener(OnAnimationEnd);
                if(TryGetComponent<AnimationResponses>(out var responses))
                {
                    var context2 = new AnimationContext()
                    {
                        TargetUnit = Executor
                    };
                    responses.Respond(AnimationResponseType.TargetPowerBasedAttacked, context2);
                }
                Action.Execute(target);
                Complete();
            }
        }

        public IActionCommand GenerateCommand(Unit target)
        {
            return new TargetedUniversalCommand<Unit, MonoTargetPowerBasedAttackAction, Unit>
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
