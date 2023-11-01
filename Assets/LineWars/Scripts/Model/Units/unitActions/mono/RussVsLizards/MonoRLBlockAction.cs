using System;

namespace LineWars.Model
{
    public class MonoRLBlockAction :
        MonoUnitAction<RLBlockAction<Node, Edge, Unit, Owned, BasePlayer>>,
        IRLBlockAction<Node, Edge, Unit, Owned, BasePlayer>
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
        public ICommandWithCommandType GenerateCommand() => new RLBlockCommand<Node, Edge, Unit, Owned, BasePlayer>(this);

        protected override RLBlockAction<Node, Edge, Unit, Owned, BasePlayer> GetAction()
        {
            var action = new RLBlockAction<Node, Edge, Unit, Owned, BasePlayer>(Unit);
            action.CanBlockChanged += (before, after) => CanBlockChanged?.Invoke(before, after);
            return action;
        }
        
        public override void Accept(IMonoUnitVisitor visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IIUnitActionVisitor<TResult, Node, Edge, Unit, Owned, BasePlayer> visitor) => visitor.Visit(this);
    }
}