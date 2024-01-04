using System.Collections.Generic;
using UnityEngine;

namespace GraphEditor
{
    public interface IMonoGraph<TNode, TEdge>
        where TNode : MonoBehaviour, IMonoNode<TNode, TEdge>
        where TEdge : MonoBehaviour, IMonoEdge<TNode, TEdge>
    {
        GameObject NodesParent { get; set; }
        GameObject EdgesParent { get; set; }

        public IDictionary<int, TNode> IdToNode { get; }
        public IDictionary<int, TEdge> IdToEdge { get; }

        public IReadOnlyList<TNode> Nodes { get; }
        public IReadOnlyList<TEdge> Edges { get; }
    }
}