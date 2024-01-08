namespace LineWars.Model
{
    public class BlowWithSwingAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IBlowWithSwingAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public override CommandType CommandType => CommandType.BlowWithSwing;


        public bool IsAvailable(TUnit target)
        {
            return ActionPointsCondition()
                   && target.Node.GetLine(Executor.Node) != null
                   && target.OwnerId != Executor.OwnerId;
        }

        public void Execute(TUnit target)
        {
            foreach (var neighbor in Executor.Node.GetNeighbors())
            {
                if (neighbor.AllIsFree)
                    continue;
                if (neighbor.OwnerId == Executor.OwnerId)
                    continue;
                foreach (var unit in neighbor.Units)
                {
                    unit.DealDamageThroughArmor(Executor.CurrentPower);
                }
            }

            CompleteAndAutoModify();
        }

        public BlowWithSwingAction(TUnit executor) : base(executor)
        {
        }

        public override void Accept(IBaseUnitActionVisitor<TNode, TEdge, TUnit> visitor)
        {
            visitor.Visit(this);
        }

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor)
        {
            return visitor.Visit(this);
        }
    }
}