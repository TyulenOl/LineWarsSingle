using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace LineWars.Model
{
    public class GraphData: ScriptableObject
    {
        [Serializable]
        private class Edge
        {
            [SerializeField] public int First;
            [SerializeField] public int Second;

            public Edge(int first, int second)
            {
                First = first;
                Second = second;
            }
        }
        
        [SerializeField] [HideInInspector] private Vector2[] nodes;
        [SerializeField] [HideInInspector] private Edge[] edges;
        [SerializeField] [HideInInspector] private List<int> spawnNodeIndexes;

        private int currentNodeIndex;
        private int currentEdgeIndex;
        private List<INode> graph;
        private HashSet<INode> spawnNodes;

        public int EdgesCount => edges.Length;
        public int NodesCount => nodes.Length;
        public int SpawnCount => spawnNodeIndexes.Count;
        
        public Vector2[] NodesPositions => nodes.ToArray();

        public (int,int)[] Edges => edges
            .Select(x => (x.First,x.Second))
            .ToArray();

        public IReadOnlyList<int> SpawnNodeIndexes => spawnNodeIndexes;

        public void Initialize(IReadOnlyCollection<INode> graph, IReadOnlyCollection<INode> spawnNodes)
        {
            var nodesCount = graph.Count;
            nodes = new Vector2[nodesCount];
            
            
            var edgesCount = graph
                .SelectMany(x => x.Edges)
                .Distinct()
                .Count();

            edges = new Edge[edgesCount];
            
            
            this.graph = graph.ToList();
            this.spawnNodes = spawnNodes.ToHashSet();

            spawnNodeIndexes = new List<int>();
            
            while (this.graph.Count > 0)
            {
                WidthTraversal(this.graph.First());
            }
        }
        
        private void WidthTraversal(INode prop)
        {
            var queue = new Queue<INode>();
            var nodeRegister = new Dictionary<INode, int>();
            var openedNodes = new HashSet<INode>();

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
                        edges[currentEdgeIndex] = new Edge(otherNodeIndex, currentNodeIndex);
                        currentEdgeIndex++;
                    }
                }

                currentNodeIndex++;
            }
        }
    }
}