using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DataStructures.PriorityQueue;
using LineWars.Extensions;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoGraph : MonoBehaviour,
        IGraphForGame<Node, Edge, Unit, Owned, BasePlayer, Nation>
    {
        private GraphForGame<Node, Edge, Unit, Owned, BasePlayer, Nation> modelGraph;
        public static MonoGraph Instance { get; set; }

        [field: SerializeField] public GameObject NodesParent { get; set; }
        [field: SerializeField] public GameObject EdgesParent { get; set; }

        private Node[] allNodes;
        private Edge[] allEdges;
        private List<SpawnInfo> spawnInfos;

        public IReadOnlyList<Node> Nodes => allNodes;
        public IReadOnlyList<Edge> Edges => allEdges;

        public IReadOnlyList<SpawnInfo> Spawns => spawnInfos;

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
            allNodes = FindObjectsOfType<Node>();
            allEdges = FindObjectsOfType<Edge>();
            GenerateSpawnInfo();
            modelGraph = new GraphForGame<Node, Edge, Unit, Owned, BasePlayer, Nation>(allNodes, allEdges);
        }

        private void GenerateSpawnInfo()
        {
            var spawns = FindObjectsOfType<Spawn>();

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


        public Dictionary<Node, bool> GetVisibilityInfo(BasePlayer player) =>
            Instance.modelGraph.GetVisibilityInfo(player);

        public List<Node> FindShortestPath(
            [NotNull] Node start,
            [NotNull] Node end,
            Func<Node, Node, bool> condition = null)
            => Instance.modelGraph.FindShortestPath(start, end, condition);

        public IEnumerable<Node> GetNodesInRange(
            Node startNode,
            uint range,
            Func<Node, Node, bool> condition = null)
            => Instance.modelGraph.GetNodesInRange(startNode, range, condition);
    }
}