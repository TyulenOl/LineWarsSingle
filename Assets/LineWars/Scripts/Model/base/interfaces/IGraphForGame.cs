using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public interface IGraphForGame<TNode, TEdge, TUnit> : 
        IGraph<TNode, TEdge>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public IEnumerable<TNode> GetVisibilityNodes(IEnumerable<TNode> ownedNodes);
    }
}