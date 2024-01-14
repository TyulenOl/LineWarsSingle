namespace LineWars.Model
{
    public class MonoPowerBasedHealAction :
        MonoUnitAction<PowerBasedHealAction<Node, Edge, Unit>>,
        ITargetedAction<Unit>,
        IStunAttackAction<Node, Edge, Unit>
    {
        protected override PowerBasedHealAction<Node, Edge, Unit> GetAction()
        {
            return new PowerBasedHealAction<Node, Edge, Unit>(Executor);
        }

        public void Execute(Unit target)
        {
            Action.Execute(target);       
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
