namespace LineWars.Model
{
    public class MonoStunAttackAction : 
        MonoUnitAction<StunAttackAction<Node, Edge, Unit>>,
        ITargetedAction<Unit>,
        IStunAttackAction<Node, Edge, Unit>
    {
        protected override StunAttackAction<Node, Edge, Unit> GetAction()
        {
            return new StunAttackAction<Node, Edge, Unit>(Executor);
        }

        public bool IsAvailable(Unit target)
        {
            return Action.IsAvailable(target);
        }

        public void Execute(Unit target)
        {
            Action.Execute(target);
        }

        public IActionCommand GenerateCommand(Unit target)
        {
            return new TargetedUniversalCommand<Unit, MonoStunAttackAction, Unit>(this, target);
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
