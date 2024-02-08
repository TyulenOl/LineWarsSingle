using UnityEngine;

namespace LineWars.Model
{
    public class MonoUpArmorAction :
        MonoUnitAction<UpArmorAction<Node, Edge, Unit>>,
        ITargetedAction<Unit>,
        IUpArmorAction<Node, Edge, Unit>
    {
        [SerializeField] private UnitAnimation unitAnimation;

        protected override bool NeedAutoComplete => false;
        protected override UpArmorAction<Node, Edge, Unit> GetAction()
        {
            return new UpArmorAction<Node, Edge, Unit>(Executor);
        }

        public bool IsAvailable(Unit target)
        {
            return Action.IsAvailable(target);
        }

        public void Execute(Unit target)
        {
            if (unitAnimation == null)
            {
                ExecuteInstant(target);
                return;
            }

            ExecuteWithAnimation(target);
        }

        private void ExecuteInstant(Unit target)
        {
            Action.Execute(target);
            Complete();
        }

        private void ExecuteWithAnimation(Unit target)
        {
            unitAnimation.Ended.AddListener(OnAnimationEnd);
            var context = new AnimationContext()
            {
                TargetUnit = target
            };
            unitAnimation.Execute(context);
            void OnAnimationEnd(UnitAnimation unitAnimation)
            {
                unitAnimation.Ended.RemoveListener(OnAnimationEnd);
                if(target.TryGetComponent<AnimationResponses>(out var responses))
                {
                    var context2 = new AnimationContext()
                    {
                        TargetUnit = Executor
                    };
                    responses.Respond(AnimationResponseType.UpArmored, context2);
                }
                ExecuteInstant(target);
            }
        }

        public IActionCommand GenerateCommand(Unit target)
        {
            return new TargetedUniversalCommand<Unit, MonoUpArmorAction, Unit>(Executor, target);
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
