﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GraphEditor;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoGraph : MonoBehaviour, IGraphForGame<Node, Edge, Unit>, IMonoGraph<Node, Edge>
    {
        private GraphForGame<Node, Edge, Unit> modelGraph;
        public static MonoGraph Instance { get; private set; }

        [field: SerializeField] public GameObject NodesParent { get; set; }
        [field: SerializeField] public GameObject EdgesParent { get; set; }
        
        private readonly Dictionary<int, Node> idToNode = new();
        private readonly Dictionary<int, Edge> idToEdge = new();
        private List<SpawnInfo> spawnInfos;

        public IReadOnlyList<Node> Nodes => idToNode.Values.ToArray();
        public IReadOnlyList<Edge> Edges => idToEdge.Values.ToArray();
        
        public IReadOnlyList<SpawnInfo> Spawns => spawnInfos;


        public IDictionary<int, Node> IdToNode => idToNode;
        public IDictionary<int, Edge> IdToEdge => idToEdge;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Debug.LogError("Более одного графа!");
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            var allNodes = FindObjectsOfType<Node>();
            var allEdges = FindObjectsOfType<Edge>();
            GenerateSpawnInfo();
            modelGraph = new GraphForGame<Node, Edge, Unit>(allNodes, allEdges);

            foreach (var node in allNodes)
                idToNode.Add(node.Id, node);
            foreach (var edge in allEdges)
                idToEdge.Add(edge.Id, edge);
        }

        private void GenerateSpawnInfo()
        {
            var spawns = FindObjectsOfType<PlayerBuilder>();

            var initialInfos = FindObjectsOfType<Node>();


            spawnInfos = new List<SpawnInfo>(spawns.Length);

            for (var i = 0; i < spawns.Length; i++)
            {
                var spawn = spawns[i];
                var group = initialInfos
                    .Where(x => x.ReferenceToSpawn == spawn)
                    .ToArray();
                var spawnInfo = new SpawnInfo(i, spawn, group);
                spawnInfos.Add(spawnInfo);
            }
        }


        public Dictionary<Node, bool> GetVisibilityInfo(BasePlayer player)
            => modelGraph.GetVisibilityInfo(player.MyNodes);

        public IEnumerable<Node> GetVisibilityNodes(IEnumerable<Node> ownedNodes)
            => modelGraph.GetVisibilityNodes(ownedNodes);

        public List<Node> FindShortestPath(
            [NotNull] Node start,
            [NotNull] Node end,
            Func<Node, Node, bool> condition = null)
        {
            return modelGraph.FindShortestPath(start, end, condition);
        }

        public IEnumerable<Node> GetNodesInRange(
            Node startNode,
            uint range,
            Func<Node, Node, bool> condition = null)
        {
            return modelGraph.GetNodesInRange(startNode, range, condition);
        }

        public IEnumerable<Node> MultiStartsLimitedBfs(
            IReadOnlyDictionary<Node, int> startNodes,
            IReadOnlyDictionary<Node, int> interferingNodes)
        {
            return modelGraph.MultiStartsLimitedBfs(startNodes, interferingNodes);
        }
    }
}