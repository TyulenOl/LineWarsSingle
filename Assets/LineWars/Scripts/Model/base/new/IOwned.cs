namespace LineWars.Model
{
    public interface IOwned<TNode, TEdge, TUnit, TOwned, TPlayer>
        
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer>
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