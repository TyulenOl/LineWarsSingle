namespace LineWars.Model
{
    public class BlowWithSwingAction<TNode, TEdge, TUnit, TOwned, TPlayer> :
            UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>,
            IBlowWithSwingAction<TNode, TEdge, TUnit, TOwned, TPlayer>

        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion
    {
        public BlowWithSwingAction(TUnit unit, MonoBlowWithSwingAction data) : base(unit, data)
        {
            Damage = data.InitialDamage;
        }

        public BlowWithSwingAction(TUnit unit, BlowWithSwingAction<TNode, TEdge, TUnit, TOwned, TPlayer> data) : base(unit, data)
        {
            Damage = data.Damage;
        }

        public override CommandType CommandType => CommandType.BlowWithSwing;

        public override void Accept(IUnitActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> visitor) =>
            visitor.Visit(this);

        public ICommand GenerateCommand() => new BlowWithSwingCommand<TNode, TEdge, TUnit, TOwned, TPlayer>(this);

        public int Damage { get; }
        public bool CanBlowWithSwing() => ActionPointsCondition();

        public void ExecuteBlowWithSwing()
        {
            foreach (var neighbor in MyUnit.Node.GetNeighbors())
            {
                if (neighbor.AllIsFree)
                    continue;
                if (neighbor.Owner == MyUnit.Owner)
                    continue;
                foreach (var unit in neighbor.Units)
                {
                    unit.DealDamageThroughArmor(Damage); 
                }
            }
            CompleteAndAutoModify();
        }
    }
}