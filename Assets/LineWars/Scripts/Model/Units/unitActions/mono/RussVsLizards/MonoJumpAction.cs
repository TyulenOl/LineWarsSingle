using UnityEngine;

namespace LineWars.Model
{
    public class MonoJumpAction :
        MonoUnitAction<JumpAction<Node, Edge, Unit>>,
        IJumpAction<Node, Edge, Unit>
    {
        [SerializeField, Min(0)] private int minJumpDistance;
        [SerializeField, Min(0)] private int maxJumpDistance;
        [SerializeField] private UnitAnimation jumpAnimation;

        public int MinJumpDistance => minJumpDistance;
        public int MaxJumpDistance => maxJumpDistance;
        protected override bool NeedAutoComplete => false;

        protected override JumpAction<Node, Edge, Unit> GetAction()
        {
            return new JumpAction<Node, Edge, Unit>(Executor, minJumpDistance, maxJumpDistance);
        }

        public bool IsAvailable(Node target)
        {
            return Action.IsAvailable(target);
        }

        public void Execute(Node target)
        {
            if(jumpAnimation == null)
                ExecuteInstant(target);
            else
                ExecuteAnimation(target);   
        }

        private void ExecuteInstant(Node target)
        {
            Action.Execute(target);
            Complete();
        }

        private void ExecuteAnimation(Node target)
        {
            var context = new AnimationContext()
            {
                TargetPosition = target.Position,
                TargetNode = target
            };

            jumpAnimation.Ended.AddListener(OnAnimEnd);
            jumpAnimation.Execute(context);
            void OnAnimEnd(UnitAnimation _)
            {
                jumpAnimation.Ended.RemoveListener(OnAnimEnd);
                ExecuteInstant(target);
            }
        }

        public IActionCommand GenerateCommand(Node target)
        {
            return new TargetedUniversalCommand<Unit,
                MonoJumpAction,
                Node>(this, target);
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
