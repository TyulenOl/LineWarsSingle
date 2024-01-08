using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public interface IGraph<TNode, TEdge> 
        where TNode : class, INode<TNode, TEdge>
        where TEdge : class, IEdge<TNode, TEdge>
    {
        public INodeIndexer<TNode, TEdge> IdToNode { get; }
        public IEdgeIndexer<TNode, TEdge> IdToEdge { get; }
        
        public IReadOnlyList<TNode> Nodes { get; }
        public IReadOnlyList<TEdge> Edges { get; }

        public void AddNode(TNode node);
        public bool RemoveNode(TNode node);
        public bool RemoveNode(int nodeId);
        public bool ContainsNode(TNode node);
        public bool ContainsNode(int nodeId);

        public void AddEdge(TEdge edge);
        public bool RemoveEdge(TEdge edge);
        public bool RemoveEdge(int edgeId);
        public bool ContainsEdge(TEdge edge);
        public bool ContainsEdge(int edgeId);


        public List<TNode> FindShortestPath(
            [NotNull] TNode start,
            [NotNull] TNode end,
            Func<TNode, TNode, bool> condition = null);

        public IEnumerable<TNode> GetNodesInRange(
            [NotNull] TNode startNode,
            uint range,
            Func<TNode, TNode, bool> condition = null);

        public IEnumerable<TNode> MultiStartsLimitedBfs(
            IReadOnlyDictionary<TNode, int> startNodes,
            IReadOnlyDictionary<TNode, int> interferingNodes);

        public IEnumerable<(TNode, uint)> FindDistanceToNodes(TNode rootNode);

        public HashSet<(TNode, TNode)> FindMostRemoteNodes();
    }
}