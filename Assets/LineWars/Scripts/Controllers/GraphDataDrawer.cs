using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LineWars.Model;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LineWars.Controllers
{
    public class GraphDataDrawer
    {
        private List<Node> allNodes;
        private List<Edge> allEdges;
        private Stack<Node> spawnNodes;
        
        private GameObject nodePrefab;
        private GameObject edgePrefab;

        private GameObject graph;

        private GraphData graphData;

        public GraphDataDrawer()
        {
            nodePrefab = Resources.Load<GameObject>("Prefabs/Node");
            edgePrefab = Resources.Load<GameObject>("Prefabs/Line");
        }

        public void DrawGraph([NotNull] GraphData graphData)
        {
            if (graphData == null) throw new ArgumentNullException(nameof(graphData));
            
            this.graphData = graphData;
            graph = Object.FindObjectOfType<Graph>().gameObject;
            if (graph == null)
            {
                graph = new GameObject("Graph");
                graph.AddComponent<Graph>();
            }

            allNodes = new List<Node>(graphData.NodesCount);
            allEdges = new List<Edge>(graphData.EdgesCount);
            spawnNodes = new Stack<Node>(graphData.SpawnCount);
            
            
            var nodeIndex = 0;
            foreach (var nodePos in graphData.NodesPositions)
            {
                var instance = Object.Instantiate(nodePrefab);
                var node = instance.GetComponent<Node>();
                node.Index = nodeIndex;
                InitializeNode(node);

                if (IsSpawn(node))
                {
                    spawnNodes.Push(node);
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