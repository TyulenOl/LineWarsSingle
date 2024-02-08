using UnityEngine;

namespace LineWars.Model
{
    public class MonoHealSacrificeAction :
        MonoUnitAction<HealSacrificeAction<Node, Edge, Unit>>,
        IHealSacrificeAction<Node, Edge, Unit>
    {
        [SerializeField] private UnitAnimation unitAnimation;

        protected override bool NeedAutoComplete => false;
        protected override HealSacrificeAction<Node, Edge, Unit> GetAction()
        {
            return new HealSacrificeAction<Node, Edge, Unit>(Executor);
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

        private void ExecuteInstant(Unit target)
        {
            Action.Execute(target);
            Complete();
        }

        public IActionCommand GenerateCommand(Unit target)
        {
            return new TargetedUniversalCommand<
                Unit,
                MonoHealSacrificeAction,
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
