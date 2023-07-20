using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    public class Graph: MonoBehaviour
    {
        private Node[] allNodes;
        private Edge[] allEdges;
        private SpawnInfo[] spawnInfos;
 
        public void Initialize(
            IEnumerable<Node> allNodes,
            IEnumerable<Edge> allEdges,
            IEnumerable<SpawnInfo> spawnInfos)
        {
            this.allNodes = allNodes.ToArray();
            this.allEdges = allEdges.ToArray();
            this.spawnInfos = spawnInfos.ToArray();
        }

        public IReadOnlyCollection<Node> AllNodes => allNodes;
        public IReadOnlyCollection<Edge> AllEdges => allEdges;
        public IReadOnlyCollection<SpawnInfo> SpawnInfos => spawnInfos;
    }
}