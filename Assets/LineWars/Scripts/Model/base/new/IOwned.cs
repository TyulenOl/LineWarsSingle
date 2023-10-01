namespace LineWars.Model
{
    public interface IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TPlayer: class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TNation: class, INation<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        #endregion
    {
        public TPlayer Owner { get; set; }

        public void ConnectTo(TPlayer basePlayer)
        {
            basePlayer.AddOwned((TOwned) this);
            Owner = basePlayer;
        }
    }
}