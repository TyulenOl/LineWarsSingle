namespace LineWars.Model
{
    public interface IUnitActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer>
        #region Ñonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion 
    {
        public void Visit(BuildAction<TNode, TEdge, TUnit, TOwned, TPlayer> action);
        public void Visit(BlockAction<TNode, TEdge, TUnit, TOwned, TPlayer> action);
        public void Visit(MoveAction<TNode, TEdge, TUnit, TOwned, TPlayer> action);
        public void Visit(HealAction<TNode, TEdge, TUnit, TOwned, TPlayer> action);
        public void Visit(DistanceAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> action);
        public void Visit(ArtilleryAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> action);
        public void Visit(MeleeAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> action);
    }
}

