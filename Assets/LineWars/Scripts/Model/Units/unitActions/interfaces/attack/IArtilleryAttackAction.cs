namespace LineWars.Model
{
    public interface IArtilleryAttackAction<TNode, TEdge, TUnit> : 
        IDistanceAttackAction<TNode, TEdge, TUnit>
    
        #region Сonstraints
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit> 
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
        #endregion 
    {
        
    }
}