namespace LineWars.Model
{
    public class MonoTargetPowerBasedAttackAction :
        MonoUnitAction<TargetPowerBasedAttackAction<Node, Edge, Unit>>,
        ITargetedAction<Unit>,
        ITargetPowerBasedAttackAction<Node, Edge, Unit>
    {
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
            Action.Execute(target);
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
