using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LineWars.Model;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LineWars
{
    public class GraphBuilder
    {
        private List<Node> allNodes;
        private List<Edge> allEdges;
        private List<Node> spawnNodes;
        
        private GameObject nodePrefab;
        private GameObject edgePrefab;

        private Graph graph;

        private GraphData graphData;
        
        public GraphBuilder()
        {
            nodePrefab = Resources.Load<GameObject>("Prefabs/Node");
            edgePrefab = Resources.Load<GameObject>("Prefabs/Line");
        }

        public Graph BuildGraph([NotNull] GraphData graphData)
        {
            if (graphData == null) throw new ArgumentNullException(nameof(graphData));
            
            this.graphData = graphData;
            graph = Object.FindObjectOfType<Graph>();
            if (graph == null)
            {
                graph = new GameObject("Graph").AddComponent<Graph>();
            }

            allNodes = new List<Node>(graphData.NodesCount);
            allEdges = new List<Edge>(graphData.EdgesCount);
            spawnNodes = new List<Node>(graphData.SpawnCount);
            
            
            var nodeIndex = 0;
            foreach (var nodePos in graphData.NodesPositions)
            {
                var instance = Object.Instantiate(nodePrefab);
                var node = instance.GetComponent<Node>();
                node.Index = nodeIndex;
                InitializeNode(node);

                if (IsSpawn(node))
                {
                    spawnNodes.Add(node);
                    node.IsSpawn = true;
                }

                allNodes.Add(node);
                nodeIndex++;
            }

            var edgeIndex = 0;
            foreach (var tuple in graphData.Edges)
            {
                var instance = Object.Instantiate(edgePrefab);
                var edge = instance.GetComponent<Edge>();
                edge.Index = edgeIndex;
                InitializeEdge(edge);

                allEdges.Add(edge);
                edgeIndex++;
            }

            graph.Initialize(allNodes, allEdges, spawnNodes);
            return graph;
        }
        
        
        private void InitializeNode(Node node)
        {
            node.Initialize();
            node.transform.SetParent(graph.transform);
            node.transform.position = graphData.NodesPositions[node.Index];
        }


        private void InitializeEdge(Edge edge)
        {
            edge.transform.SetParent(graph.transform);

            var edgeData = graphData.Edges[edge.Index];

            var node1 = allNodes[edgeData.Item1];
            var node2 = allNodes[edgeData.Item2];

            edge.Initialize(node1, node2);
            node1.AddEdge(edge);
            node2.AddEdge(edge);
        }
        
        private bool IsSpawn(Node node) => graphData.SpawnNodeIndexes.Contains(node.Index);
    }
}