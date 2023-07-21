using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace LineWars.Model
{
    public class GraphData : ScriptableObject
    {
        [SerializeField] [HideInInspector] private Vector2[] nodes;
        [SerializeField] [HideInInspector] private Vector2Int[] edges;

        [FormerlySerializedAs("edgeDatas")]
        [SerializeField] [HideInInspector] private List<AdditionalEdgeData> additionalEdgeDatas;
        
        [FormerlySerializedAs("nodeDatas")]
        [SerializeField] [HideInInspector] private List<AdditionalNodeData> additionalNodeDatas;

        //служебные поля
        private int currentNodeIndex;
        private int currentEdgeIndex;

        private List<Node> allNodes;
        private List<Edge> allEdges;

        private List<Node> tempCopyNodes;

        private Dictionary<NodeInitialInfo, int> pairNodeInfoAndSpawnIndex;

        public int EdgesCount => edges.Length;
        public int NodesCount => nodes.Length;
        public int SpawnCount => additionalNodeDatas.Count(x=> x.IsSpawn);

        public Vector2[] NodesPositions => nodes.ToArray();

        public (int, int)[] Edges => edges
            .Select(vector2Int => (vector2Int.x, vector2Int.y))
            .ToArray();
        public AdditionalEdgeData GetAdditionalEdgeData(int index)
        {
            if (
                additionalEdgeDatas != null
                && 0 <= index && index < additionalEdgeDatas.Count
            )
                return additionalEdgeDatas[index];
            return new AdditionalEdgeData();
        }
        
        public AdditionalNodeData GetAdditionalNodeData(int index)
        {
            if (
                additionalNodeDatas != null
                && 0 <= index && index < additionalNodeDatas.Count
            )
                return additionalNodeDatas[index];
            return new AdditionalNodeData();
        }

        public void Initialize(IReadOnlyCollection<Node> graph)
        {
            allNodes = graph.ToList();
            allEdges = graph
                .SelectMany(x => x.Edges)
                .Distinct()
                .ToList();
            
            InitializeBaseDada();
            InitializeAdditionalData();
        }

        private void InitializeBaseDada()
        {
            currentNodeIndex = 0;
            currentEdgeIndex = 0;
            nodes = new Vector2[allNodes.Count];
            edges = new Vector2Int[allEdges.Count];

            tempCopyNodes = allNodes.ToList();
            
            while (tempCopyNodes.Count > 0)
            {
                WidthTraversal(tempCopyNodes.First());
            }
        }

        private void WidthTraversal(Node prop)
        {
            var queue = new Queue<Node>();
            var nodeRegister = new Dictionary<Node, int>();
            var openedNodes = new HashSet<Node>();

            queue.Enqueue(prop);
            openedNodes.Add(prop);

            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                tempCopyNodes.Remove(currentNode);
                
                nodes[currentNodeIndex] = currentNode.Position;

                nodeRegister.Add(currentNode, currentNodeIndex);

                foreach (var edge in currentNode.Edges)
                {
                    var otherNode = edge.GetOther(currentNode);
                    if (!nodeRegister.ContainsKey(otherNode))
                    {
                        if (!openedNodes.Contains(otherNode))
                        {
                            queue.Enqueue(otherNode);
                            openedNodes.Add(otherNode);
                        }
                    }
                    else
                    {
                        var otherNodeIndex = nodeRegister[otherNode];
                        edges[currentEdgeIndex] = new Vector2Int(otherNodeIndex, currentNodeIndex);
                        currentEdgeIndex++;
                    }
                }

                currentNodeIndex++;
            }
        }

        private void InitializeAdditionalData()
        {
            additionalNodeDatas = new List<AdditionalNodeData>(allNodes.Count);
            additionalEdgeDatas = new List<AdditionalEdgeData>(allEdges.Count);

            FindAllSpawns();
            
            foreach (var node in allNodes)
                additionalNodeDatas.Add(GetAdditionalNodeData(node));

            foreach (var edge in allEdges)
                additionalEdgeDatas.Add(GetAdditionalEdgeData(edge));
        }

        private void FindAllSpawns()
        {
            pairNodeInfoAndSpawnIndex = new Dictionary<NodeInitialInfo, int>();
            var spawnIndex = 0;
            foreach (var node in allNodes)
            {
                if (node.TryGetComponent<NodeInitialInfo>(out var info) && info.IsSpawn)
                {
                    pairNodeInfoAndSpawnIndex.Add(info, spawnIndex);
                    spawnIndex++;
                }
            }
        }
        
        private AdditionalEdgeData GetAdditionalEdgeData(Edge edge)
        {
            return new AdditionalEdgeData(edge.LineType);
        }

        private AdditionalNodeData GetAdditionalNodeData(Node node)
        {
            if (node.TryGetComponent<NodeInitialInfo>(out var info) 
                && (info.IsSpawn || info.ReferenceToSpawn != null))
            {
                var ownedIndex = info.IsSpawn
                    ? pairNodeInfoAndSpawnIndex[info]
                    : pairNodeInfoAndSpawnIndex[info.ReferenceToSpawn];
                return new AdditionalNodeData
                (
                    ownedIndex,
                    info.IsSpawn,
                    info.LeftUnitPrefab,
                    info.RightUnitPrefab,
                    info.GroupColor
                );
            }

            return new AdditionalNodeData();
        }

        public void RefreshFrom(GraphData graphData)
        {
            if (graphData == null)
                return;
            nodes = graphData.nodes;
            edges = graphData.edges;
            additionalEdgeDatas = graphData.additionalEdgeDatas;
            additionalNodeDatas = graphData.additionalNodeDatas;
        }
    }
}