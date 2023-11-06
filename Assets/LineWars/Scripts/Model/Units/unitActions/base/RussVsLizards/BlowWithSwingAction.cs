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

        public ICommandWithCommandType GenerateCommand() => new BlowWithSwingCommand<TNode, TEdge, TUnit>(this);

        public int Damage { get; }
        public bool CanBlowWithSwing() => ActionPointsCondition();

        public void ExecuteBlowWithSwing()
        {
            foreach (var neighbor in MyUnit.Node.GetNeighbors())
            {
                if (neighbor.AllIsFree)
                    continue;
                if (neighbor.OwnerId == MyUnit.OwnerId)
                    continue;
                foreach (var unit in neighbor.Units)
                {
                    unit.DealDamageThroughArmor(Damage); 
                }
            }
            CompleteAndAutoModify();
        }

        public BlowWithSwingAction(TUnit executor, int damage) : base(executor)
        {
            Damage = damage;
        }
        
        public override void Accept(IUnitActionVisitor<TNode, TEdge, TUnit> visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IIUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor) => visitor.Visit(this);
    }
}