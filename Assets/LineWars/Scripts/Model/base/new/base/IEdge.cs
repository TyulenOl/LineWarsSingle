using System;

namespace LineWars.Model
{
    public interface IEdge<TNode, TEdge>
        where TEdge : IEdge<TNode, TEdge> 
        where TNode : INode<TNode, TEdge>
    {
        TNode FirstNode { get; }
        TNode SecondNode { get; }
        
        public bool IsIncident(TNode node) => FirstNode.Equals(node) || SecondNode.Equals(node);

        public TNode GetOther(TNode node)
        {
            if (!IsIncident(node)) throw new ArgumentException();
            return FirstNode.Equals(node) ? SecondNode : FirstNode;
        }
    }
}