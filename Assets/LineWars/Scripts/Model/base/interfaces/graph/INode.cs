using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public interface INode<TNode, TEdge>
        where TEdge : IEdge<TNode, TEdge> 
        where TNode : INode<TNode, TEdge>
    {
        IEnumerable<TEdge> Edges { get; }

        IEnumerable<TNode> GetNeighbors()
        {
            foreach (var edge in Edges)
            {
                if (edge.FirstNode.Equals(this))
                    yield return edge.SecondNode;
                else
                    yield return edge.FirstNode;
            }
        }
        
        public TEdge GetLine(TNode node) => Edges.Intersect(node.Edges).FirstOrDefault();
    }
}