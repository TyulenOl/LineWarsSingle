namespace LineWars.Model
{
    public class StunAttackAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IStunAttackAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public override CommandType CommandType => CommandType.Stun;
        public StunAttackAction(TUnit executor) : base(executor)
        {
        }

        public bool IsAvailable(TUnit target)
        {
            return ActionPointsCondition() &&
                target != null &&
                target.OwnerId != Executor.OwnerId &&
                Executor.Node.GetLine(target.Node) != null;
        }

        public void Execute(TUnit target)
        {
            Stun(target);
        }

        private void Stun(TUnit target)
        {
            target.CurrentActionPoints -= Executor.CurrentPower;
            CompleteAndAutoModify();
        }

        public IActionCommand GenerateCommand(TUnit target)
        {
            return new TargetedUniversalCommand
                <TUnit,
                StunAttackAction<TNode, TEdge, TUnit>,
                TUnit>(this, target);
        }
        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Accept(IBaseUnitActionVisitor<TNode, TEdge, TUnit> visitor)
        {
            visitor.Visit(this);
        }
    }
}
