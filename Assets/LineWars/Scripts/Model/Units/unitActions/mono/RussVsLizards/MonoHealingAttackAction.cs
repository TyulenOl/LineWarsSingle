namespace LineWars.Model
{
    public class MonoHealingAttackAction :
        MonoUnitAction<HealingAttackAction<Node, Edge, Unit>>,
        ITargetedAction<Unit>,
        IHealingAttackAction<Node, Edge, Unit>
    {
        protected override HealingAttackAction<Node, Edge, Unit> GetAction()
        {
            return new HealingAttackAction<Node, Edge, Unit>(Executor);
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
            return new TargetedUniversalCommand<Unit, MonoHealingAttackAction, Unit>
                (this, Executor);
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
