using System;

namespace LineWars.Model
{
    public interface IEdgeForGame<TNode, TEdge, TUnit> :
        ITargetedAlive,
        IEdge<TNode, TEdge>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public int Id { get; }
        public LineType LineType { get; set; }
    }
}