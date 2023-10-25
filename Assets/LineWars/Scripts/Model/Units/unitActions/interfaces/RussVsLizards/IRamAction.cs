namespace LineWars.Model
{
    public interface IRamAction<TNode, TEdge, TUnit, TOwned, TPlayer> :
        IUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>,
        ITargetedAction,
        IActionWithDamage
        
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion

    {
        public bool CanRam(TNode node);
        public void Ram(TNode node);
    }
}