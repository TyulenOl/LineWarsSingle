using System;

namespace LineWars.Model
{
    public class MonoRLBlockAction :
        MonoUnitAction<RLBlockAction<Node, Edge, Unit>>,
        IRLBlockAction<Node, Edge, Unit>
    {
        private void OnDestroy()
        {
            CanBlockChanged = null;
        }

        public bool IsBlocked => Action.IsBlocked;
        public event Action<bool, bool> CanBlockChanged;

        public bool CanBlock() => Action.CanBlock();
        public void EnableBlock()
        {
            //TODO: анимации и звуки
            Action.EnableBlock();
        }

        protected override RLBlockAction<Node, Edge, Unit> GetAction()
        {
            var action = new RLBlockAction<Node, Edge, Unit>(Unit);
            action.CanBlockChanged += (before, after) => CanBlockChanged?.Invoke(before, after);
            return action;
        }
        
        public override void Accept(IMonoUnitActionVisitor visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor) => visitor.Visit(this);
    }
}