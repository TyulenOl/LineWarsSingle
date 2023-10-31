namespace LineWars.Model
{
    public interface IIUnitActionVisitor<out TResult, TNode, TEdge, TUnit, TOwned, TPlayer>
        
        #region Constraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion
    {
        public TResult Visit(IBuildAction<TNode, TEdge, TUnit, TOwned, TPlayer> action);
        public TResult Visit(IBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer> action);
        public TResult Visit(IMoveAction<TNode, TEdge, TUnit, TOwned, TPlayer> action);
        public TResult Visit(IHealAction<TNode, TEdge, TUnit, TOwned, TPlayer> action);
        public TResult Visit(IDistanceAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> action);
        public TResult Visit(IArtilleryAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> action);
        public TResult Visit(IMeleeAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> action);
        public TResult Visit(IRLBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer> action);
        public TResult Visit(ISacrificeForPerunAction<TNode, TEdge, TUnit, TOwned, TPlayer> action);
        public TResult Visit(IRamAction<TNode, TEdge, TUnit, TOwned, TPlayer> action);
        public TResult Visit(IBlowWithSwingAction<TNode, TEdge, TUnit, TOwned, TPlayer> action);
        public TResult Visit(IShotUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer> action);
        public TResult Visit(IRLBuildAction<TNode, TEdge, TUnit, TOwned, TPlayer> action);
    }
}