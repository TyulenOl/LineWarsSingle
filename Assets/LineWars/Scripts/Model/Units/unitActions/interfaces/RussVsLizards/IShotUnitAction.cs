namespace LineWars.Model
{
    public interface IShotUnitAction<TNode, TEdge, TUnit>: 
        IUnitAction<TNode, TEdge, TUnit>,
        IMultiStageTargetAction

        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public TUnit TakenUnit { get; }

        public bool CanTakeUnit(TUnit unit);
        public void TakeUnit(TUnit unit);

        public bool CanShotUnitTo(TNode node);
        public void ShotUnitTo(TNode node);
    }
}