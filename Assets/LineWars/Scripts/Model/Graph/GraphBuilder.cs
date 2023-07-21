using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LineWars.Model
{
    public class GraphBuilder
    {
        private List<Node> allNodes;
        private List<Edge> allEdges;

        private GameObject nodePrefab;
        private GameObject edgePrefab;

        private Graph graph;

        private GraphData graphData;

        private bool isEditor;

        public GraphBuilder()
        {
            nodePrefab = Resources.Load<GameObject>("Prefabs/Node");
            edgePrefab = Resources.Load<GameObject>("Prefabs/Line");
        }

        public Graph BuildGraph([NotNull] GraphData graphData, bool isEditor = false)
        {
            if (graphData == null) throw new ArgumentNullException(nameof(graphData));

            this.graphData = graphData;
            this.isEditor = isEditor;

            graph = Object.FindObjectOfType<Graph>();
            if (graph == null)
            {
                graph = new GameObject("Graph").AddComponent<Graph>();
            }

            allNodes = new List<Node>(graphData.NodesCount);
            allEdges = new List<Edge>(graphData.EdgesCount);


            BuildNodes(graphData);
            BuildEdges(graphData);

            var spawnInfos = GetSpawnInfos();
            ForEditorBuild();
            
            graph.Initialize(allNodes, allEdges, spawnInfos);
            return graph;
        }

        private void BuildNodes(GraphData graphData)
        {
            var nodeIndex = 0;
            foreach (var _ in graphData.NodesPositions)
            {
                var instance = Object.Instantiate(nodePrefab);
                var node = instance.GetComponent<Node>();
                node.Index = nodeIndex;
                BuildNode(node);
                allNodes.Add(node);
                nodeIndex++;
            }
        }

        private void BuildEdges(GraphData graphData)
        {
            var edgeIndex = 0;
            foreach (var _ in graphData.Edges)
            {
                var instance = Object.Instantiate(edgePrefab);
                var edge = instance.GetComponent<Edge>();
                edge.Index = edgeIndex;
                BuildEdge(edge);

                allEdges.Add(edge);
                edgeIndex++;
            }
        }

        private void BuildNode(Node node)
        {
            node.Initialize();

            var nodeData = graphData.NodesPositions[node.Index];
            var additionalNodeData = graphData.GetAdditionalNodeData(node.Index);

            node.transform.SetParent(graph.transform);
            node.transform.position = nodeData;
        }

        private void BuildEdge(Edge edge)
        {
            edge.transform.SetParent(graph.transform);

            var edgeData = graphData.Edges[edge.Index];
            var additionalData = graphData.GetAdditionalEdgeData(edge.Index);

            var node1 = allNodes[edgeData.Item1];
            var node2 = allNodes[edgeData.Item2];

            edge.Initialize(node1, node2, additionalData.LineType);
            node1.AddEdge(edge);
            node2.AddEdge(edge);
        }

        private void ForEditorBuild()
        {
            var spawnsInitialInfosRegister = new Dictionary<int, NodeInitialInfo>();
            var spawns = GetSpawns();
            var noSpawns = GetNoSpawns();

            foreach (var spawn in spawns)
            {
                var initialInfo = spawn.gameObject.AddComponent<NodeInitialInfo>();
                var additionalInfo = GetAdditionalNodeData(spawn);
                spawnsInitialInfosRegister.Add(additionalInfo.OwnedIndex, initialInfo);
                initialInfo.Initialize(
                    true, 
                    null,
                    additionalInfo.LeftUnitPrefab,
                    additionalInfo.RightUnitPrefab,
                    additionalInfo.GroupColor);
            }

            foreach (var noSpawn in noSpawns)
            {
                var initialInfo = noSpawn.gameObject.AddComponent<NodeInitialInfo>();
                var additionalInfo = GetAdditionalNodeData(noSpawn);

                var referenceToSpawn = spawnsInitialInfosRegister[additionalInfo.OwnedIndex];
                initialInfo.Initialize(
                    false,
                    referenceToSpawn,
                    additionalInfo.LeftUnitPrefab,
                    additionalInfo.RightUnitPrefab,
                    additionalInfo.GroupColor);
            }
        }

        private SpawnInfo[] GetSpawnInfos()
        {
            var spawnInfoRegister = new Dictionary<int, SpawnInfo>();
            var spawns = GetSpawns();
            var noSpawns = GetNoSpawns();

            foreach (var spawn in spawns)
            {
                var additionalInfo = GetAdditionalNodeData(spawn);

                var nodeInfo = new NodeInfo
                (
                    spawn,
                    additionalInfo.LeftUnitPrefab,
                    additionalInfo.RightUnitPrefab
                );
                var spawnInfo = new SpawnInfo
                (
                    additionalInfo.OwnedIndex,
                    nodeInfo,
                    new List<NodeInfo>() {nodeInfo}
                );
                
                spawnInfoRegister.Add(additionalInfo.OwnedIndex, spawnInfo);
            }

            foreach (var noSpawn in noSpawns)
            {
                var additionalInfo = GetAdditionalNodeData(noSpawn);
                var nodeInfo = new NodeInfo
                (
                    noSpawn,
                    additionalInfo.LeftUnitPrefab,
                    additionalInfo.RightUnitPrefab
                );
                spawnInfoRegister[additionalInfo.OwnedIndex].NodeInfos.Add(nodeInfo);
            }

            return spawnInfoRegister.Values.ToArray();
        }

        private List<Node> GetNoSpawns()
        {
            return allNodes
                .Where(x =>
                {
                    var data = GetAdditionalNodeData(x);
                    return data.HasOwner && !data.IsSpawn;
                })
                .ToList();
        }

        private List<Node> GetSpawns()
        {
            return allNodes
                .Where(x =>
                {
                    var data = GetAdditionalNodeData(x);
                    return data.HasOwner && data.IsSpawn;
                })
                .ToList();
        }

        private AdditionalNodeData GetAdditionalNodeData(Node node) => graphData.GetAdditionalNodeData(node.Index);
    }
}