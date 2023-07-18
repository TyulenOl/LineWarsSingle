using System.Collections.Generic;
using System.Linq;
using LineWars.Extensions;
using UnityEngine;

namespace LineWars.Model
{
    public class Graph: MonoBehaviour
    {
        private Node[] allNodes;
        private Edge[] allEdges;

        public void Initialize(
            IEnumerable<Node> allNodes,
            IEnumerable<Edge> allEdges)
        {
            this.allNodes = allNodes.ToArray();
            this.allEdges = allEdges.ToArray();
        }

        public IReadOnlyCollection<Node> AllNodes => allNodes;
        public IReadOnlyCollection<Edge> AllEdges => allEdges;
    }
}