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

    public class SpawnInfo
    {
        public readonly int PlayerIndex;
        public readonly NodeInfo SpawnNode;
        public readonly List<NodeInfo> NodeInfos;

        public SpawnInfo(int playerIndex, NodeInfo spawnNode, List<NodeInfo> nodeInfos)
        {
            PlayerIndex = playerIndex;
            SpawnNode = spawnNode;
            NodeInfos = nodeInfos;
        }
    }

    public class NodeInfo
    {
        public readonly Node ReferenceToNode;
        public readonly Unit LeftUnitPrefab;
        public readonly Unit RightUnitPrefab;

        public NodeInfo(Node referenceToNode, Unit leftUnitPrefab, Unit rightUnitPrefab)
        {
            ReferenceToNode = referenceToNode;
            LeftUnitPrefab = leftUnitPrefab;
            RightUnitPrefab = rightUnitPrefab;
        }
    }
}