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
        private Node[] spawnNodes;
        
        public void Initialize(IEnumerable<Node> allNodes, IEnumerable<Edge> allEdges, IEnumerable<Node> spawnNodes)
        {
            this.allNodes = allNodes.ToArray();
            this.allEdges = allEdges.ToArray();
            this.spawnNodes = spawnNodes.ToArray();
        }

        public IReadOnlyCollection<Node> AllNodes => allNodes;
        public IReadOnlyCollection<Edge> AllEdges => allEdges;
        public IReadOnlyCollection<Node> SpawnNodes => spawnNodes;
    }
}