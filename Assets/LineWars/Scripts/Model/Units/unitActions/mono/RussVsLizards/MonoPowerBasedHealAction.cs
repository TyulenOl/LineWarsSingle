using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoPowerBasedHealAction :
        MonoUnitAction<PowerBasedHealAction<Node, Edge, Unit>>,
        ITargetedAction<Unit>,
        IPowerBasedHealAction<Node, Edge, Unit>
    {
        [SerializeField] private UnitAnimation unitAnimation;
        [SerializeField] private SFXData healSFX;

        protected override bool NeedAutoComplete => false;

        protected override PowerBasedHealAction<Node, Edge, Unit> GetAction()
        {
            return new PowerBasedHealAction<Node, Edge, Unit>(Executor);
        }

        public void Execute(Unit target)
        {
            if (unitAnimation != null)
                AnimationExecute(target);
            else
                ExecuteInstant(target);
        }

        private void AnimationExecute(Unit target)
        {
            var context = new AnimationContext()
            {
                TargetUnit = target
            };
            unitAnimation.Ended.AddListener(OnAnimationEnd);
            unitAnimation.Execute(context);

            void OnAnimationEnd(UnitAnimation _)
            {
                unitAnimation.Ended.RemoveListener(OnAnimationEnd);
                if(target.TryGetComponent<AnimationResponses>(out var responses)) 
                {
                    var context2 = new AnimationContext()
                    {
                        TargetUnit = Executor
                    };
                    responses.Respond(AnimationResponseType.Healed, context2);
                }
                ExecuteInstant(target);
            }
        }
        private void ExecuteInstant(Unit target)
        {
            Action.Execute(target);
            Executor.PlaySfx(healSFX);
            Complete();
        }

        public bool IsAvailable(Unit target)
        {
            return Action.IsAvailable(target); 
        }

        public IActionCommand GenerateCommand(Unit target)
        {
            return new TargetedUniversalCommand
                <Unit, MonoPowerBasedHealAction, Unit>(Executor, target);
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
