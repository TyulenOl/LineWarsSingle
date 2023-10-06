using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public interface INode<TNode, TEdge>
        where TEdge : IEdge<TNode, TEdge> 
        where TNode : INode<TNode, TEdge>
    {
        IEnumerable<TEdge> Edges { get; }
        IEnumerable<TNode> GetNeighbors();
        
        public TEdge GetLine(TNode node) => Edges.Intersect(node.Edges).FirstOrDefault();
    }
}