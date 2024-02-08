namespace LineWars.Model
{
    public class HealSacrificeAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IHealSacrificeAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public override CommandType CommandType => CommandType.HealSacrifice;

        public HealSacrificeAction(TUnit executor) : base(executor)
        {
        }

        public bool IsAvailable(TUnit target)
        {
            return ActionPointsCondition() && 
                Executor.CurrentPower > 0 &&
                target.OwnerId == Executor.OwnerId;
         }

        public void Execute(TUnit target)
        {
            foreach(var node in Executor.Node.GetNeighbors())
            {
                if (node.AllIsFree) continue;

                if (!node.LeftIsFree &&
                    node.LeftUnit.OwnerId == Executor.OwnerId &&
                    node.LeftUnit.Size == UnitSize.Little)
                    node.LeftUnit.CurrentHp += Executor.CurrentPower;

                if(!node.RightIsFree && 
                    node.RightUnit.OwnerId == Executor.OwnerId)
                    node.RightUnit.CurrentHp += Executor.CurrentPower;
            }
            Executor.CurrentHp = 0;
            CompleteAndAutoModify();
        }
        public IActionCommand GenerateCommand(TUnit target)
        {
            return new TargetedUniversalCommand<TUnit,
                HealSacrificeAction<TNode, TEdge, TUnit>,
                TUnit>(this, target);
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
