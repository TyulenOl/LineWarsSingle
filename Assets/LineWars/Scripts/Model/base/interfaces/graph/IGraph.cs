using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public interface IGraph<TNode, TEdge>
        where TNode : class, INode<TNode, TEdge>
        where TEdge : class, IEdge<TNode, TEdge>
    {
        public IReadOnlyList<TNode> Nodes { get; }
        public IReadOnlyList<TEdge> Edges { get; }

        public List<TNode> FindShortestPath(
            [NotNull] TNode start,
            [NotNull] TNode end,
            Func<TNode, TNode, bool> condition = null);

        public IEnumerable<TNode> GetNodesInRange(
            [NotNull] TNode startNode,
            uint range,
            Func<TNode, TNode, bool> condition = null);
    }
}