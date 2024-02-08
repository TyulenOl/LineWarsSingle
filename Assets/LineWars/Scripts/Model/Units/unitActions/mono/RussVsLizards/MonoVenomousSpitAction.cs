using UnityEngine;

namespace LineWars.Model
{
    public class MonoVenomousSpitAction :
        MonoUnitAction<VenomousSpitAction<Node, Edge, Unit>>,
        IVenomousSpitAction<Node, Edge, Unit>
    {
        [SerializeField] private int rounds;
        [SerializeField] private UnitAnimation unitAnimation;

        public int Rounds => rounds;

        protected override bool NeedAutoComplete => false;
        protected override VenomousSpitAction<Node, Edge, Unit> GetAction()
        {
            return new VenomousSpitAction<Node, Edge, Unit>(Executor, rounds);
        }

        public bool IsAvailable(Unit target)
        {
            return Action.IsAvailable(target);
        }

        public void Execute(Unit target)
        {
            if (unitAnimation == null)
                ExecuteInstant(target);
            else
                ExecuteAnimation(target);
        }

        private void ExecuteInstant(Unit target)
        {
            Action.Execute(target);
            Complete();
        }

        private void ExecuteAnimation(Unit target)
        {
            var context = new AnimationContext()
            {
                TargetPosition = target.transform.position,
                TargetUnit = target
            };
            unitAnimation.Ended.AddListener(OnAnimEnd);
            unitAnimation.Execute(context);

            void OnAnimEnd(UnitAnimation _)
            {
                unitAnimation.Ended.RemoveListener(OnAnimEnd);
                ExecuteInstant(target);
            }
        }

        public IActionCommand GenerateCommand(Unit target)
        {
            return new TargetedUniversalCommand<
                Unit,
                MonoVenomousSpitAction,
                Unit>(this, target);
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
