using System;

namespace LineWars.Model
{
    public class MonoRLBlockAction : MonoUnitAction, IRLBlockAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        private RLBlockAction<Node, Edge, Unit, Owned, BasePlayer> BlockAction
            => (RLBlockAction<Node, Edge, Unit, Owned, BasePlayer>) ExecutorAction;

        protected override ExecutorAction GetAction()
        {
            var action = new RLBlockAction<Node, Edge, Unit, Owned, BasePlayer>(Unit, this);
            action.CanBlockChanged += (before, after) => CanBlockChanged?.Invoke(before, after);
            return action;
        }

        public override void Accept(IMonoUnitVisitor visitor) => visitor.Visit(this);

        public ICommand GenerateCommand() => new RLBlockCommand<Node, Edge, Unit, Owned, BasePlayer>(this);

        public bool IsBlocked => BlockAction.IsBlocked;
        public event Action<bool, bool> CanBlockChanged;

        public bool CanBlock() => BlockAction.CanBlock();
        public void EnableBlock() => BlockAction.EnableBlock();
    }
}