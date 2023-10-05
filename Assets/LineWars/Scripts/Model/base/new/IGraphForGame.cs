using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public interface IGraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        : IGraph<TNode, TEdge>
        
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer>
        #endregion 
    {
        public Dictionary<TNode, bool> GetVisibilityInfo(TPlayer player);
    }
}