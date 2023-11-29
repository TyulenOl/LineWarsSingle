namespace LineWars.Model
{
    public class HealYourselfAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IHealYourselfAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public int HealAmount { get; }
        
        public HealYourselfAction(TUnit executor, int healAmount) : base(executor)
        {
            HealAmount = healAmount;
        }

        public override CommandType CommandType => CommandType.VodaBajkalskaya;
        
        public bool CanExecute()
        {
            return ActionPointsCondition();
        }

        public void Execute()
        {
            Executor.CurrentHp += HealAmount;
            CompleteAndAutoModify();
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