using System;

namespace LineWars.Model
{
    public interface IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>:
        INumbered,
        ITarget,
        IAlive,
        IEdge<TNode, TEdge>
        
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TOwned, TPlayer>
        #endregion
    {
        public LineType LineType { get; set; }
    }
}