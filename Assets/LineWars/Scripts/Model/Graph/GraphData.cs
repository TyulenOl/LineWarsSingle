using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace LineWars.Model
{
    [Serializable]
    public class AdditionalEdgeData
    {
        [SerializeField] public LineType lineType;
        public LineType LineType => lineType;

        public AdditionalEdgeData(LineType lineType)
        {
            this.lineType = lineType;
        }
    }
    
    public class GraphData: ScriptableObject
    {
        [SerializeField] [HideInInspector] private Vector2[] nodes;
        [SerializeField] [HideInInspector] private Vector2Int[] edges;
        [SerializeField] [HideInInspector] private List<int> spawnNodeIndexes;
        [FormerlySerializedAs("edgeDatas")] [SerializeField] [HideInInspector] private AdditionalEdgeData[] edgeAdditionalDatas;

        private int currentNodeIndex;
        private int currentEdgeIndex;
        private List<Node> graph;
        private HashSet<Node> spawnNodes;

        public int EdgesCount => edges.Length;
        public int NodesCount => nodes.Length;
        public int SpawnCount => spawnNodeIndexes.Count;
        
        public Vector2[] NodesPositions => nodes.ToArray();

        public (int,int)[] Edges => edges
            .Select(vector2Int => (vector2Int.x,vector2Int.y))
            .ToArray();

        public IReadOnlyList<int> SpawnNodeIndexes => spawnNodeIndexes;
        public IReadOnlyList<AdditionalEdgeData> EdgeAdditionalDatas => edgeAdditionalDatas;

        public void Initialize(IReadOnlyCollection<Node> graph)
        {
            var nodesCount = graph.Count;
            nodes = new Vector2[nodesCount];
            
            
            var edgesCount = graph
                .SelectMany(x => x.Edges)
                .Distinct()
                .Count();

            edges = new Vector2Int[edgesCount];
            edgeAdditionalDatas = new AdditionalEdgeData[edgesCount];
            
            this.graph = graph.ToList();
            this.spawnNodes = graph
                .Where(x => x.IsSpawn)
                .ToHashSet();

            spawnNodeIndexes = new List<int>();
            
            while (this.graph.Count > 0)
            {
                WidthTraversal(this.graph.First());
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
                graph.Remove(currentNode);

                if (spawnNodes.Contains(currentNode))
                    spawnNodeIndexes.Add(currentNodeIndex);
                
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
                        edgeAdditionalDatas[currentEdgeIndex] = GetAdditionalEdgeData(edge);
                        currentEdgeIndex++;
                    }
                }

                currentNodeIndex++;
            }
        }

        private AdditionalEdgeData GetAdditionalEdgeData(Edge edge)
        {
            return new AdditionalEdgeData(edge.LineType);
        }
    }
}