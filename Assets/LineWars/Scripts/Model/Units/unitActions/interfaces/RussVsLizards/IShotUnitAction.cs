namespace LineWars.Model
{
    public interface IShotUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>: 
        IUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>,
        IMultiStageTargetAction

        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion

    {
        public TUnit TakenUnit { get; }

        public bool CanTakeUnit(TUnit unit);
        public void TakeUnit(TUnit unit);

        public bool CanShotUnitTo(TNode node);
        public void ShotUnitTo(TNode node);
    }
}