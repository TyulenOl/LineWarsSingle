namespace LineWars.Model
{
    public class SpawningUnitAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        ISpawningUnitAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private CommandType commandType;
        public IUnitFabric<TNode, TEdge, TUnit> UnitFabric { get; private set; }
        public override CommandType CommandType => commandType;

        public SpawningUnitAction(TUnit executor, 
            IUnitFabric<TNode, TEdge, TUnit> unitFabric, 
            CommandType commandType) : base(executor)
        {
            this.commandType = commandType;
            this.UnitFabric = unitFabric;
        }

        public bool IsAvailable(TNode target)
        {
            var line = Executor.Node.GetLine(target);
            return ActionPointsCondition()
                   && line != null
                   && UnitFabric.CanSpawn(target);
        }

        public void Execute(TNode target)
        {
            UnitFabric.Spawn(target);
            CompleteAndAutoModify();
        }

        public IActionCommand GenerateCommand(TNode target)
        {
            return new TargetedUniversalCommand
                <TUnit,
                SpawningUnitAction<TNode, TEdge, TUnit>,
                TNode>(this, target);
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
