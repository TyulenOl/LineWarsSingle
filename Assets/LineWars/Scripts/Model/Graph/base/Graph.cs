using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.PriorityQueue;
using UnityEngine;

namespace LineWars.Model
{
    public class Graph : MonoBehaviour
    {
        private static Graph Instance { get; set; }
        
        [field: SerializeField] public GameObject NodesParent { get; set; }
        [field: SerializeField] public GameObject EdgesParent { get; set; }
        
        private Node[] allNodes;
        private Edge[] allEdges;
        private List<SpawnInfo> spawnInfos;
        private Stack<SpawnInfo> spawnInfosStack;

        public static IReadOnlyList<Node> AllNodes => Instance != null ? Instance.allNodes : Array.Empty<Node>();
        public static IReadOnlyList<Edge> AllEdges => Instance != null ? Instance.allEdges : Array.Empty<Edge>();
        
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
        }

        private void GenerateSpawnInfo()
        {
            var spawns = FindObjectsOfType<Spawn>();
            var initialInfos = FindObjectsOfType<NodeInitialInfo>();

            spawnInfos = new List<SpawnInfo>(spawns.Length);
            spawnInfosStack = new Stack<SpawnInfo>(spawns.Length);

            for (var i = 0; i < spawns.Length; i++)
            {
                var spawn = spawns[i];
                var group = initialInfos
                    .Where(x => x.ReferenceToSpawn == spawn)
                    .ToArray();
                var spawnInfo = new SpawnInfo(i, spawn.NodeInitialInfo, group);
                spawnInfos.Add(spawnInfo);
                spawnInfosStack.Push(spawnInfo);
            }
        }

        public static bool HasSpawnPoint() =>
            Instance != null
                ? Instance._HasSpawnPoint()
                : throw new NullReferenceException("Graph is not Instance!");

        private bool _HasSpawnPoint() => spawnInfosStack.Count != 0;

        public static SpawnInfo GetSpawnPoint() =>
            Instance != null
                ? Instance._GetSpawnPoint()
                : throw new NullReferenceException("Graph is not Instance!");

        private SpawnInfo _GetSpawnPoint() => spawnInfosStack.Pop();

        public static bool[] GetVisibilityInfo(Player player) =>
            Instance != null
                ? Instance._GetVisibilityInfo(player)
                : throw new NullReferenceException("Graph is not Instance!");
        
        private bool[] _GetVisibilityInfo(Player player)
        {
            var result = new bool[allNodes.Length];

            var ownedNodes = player.OwnedObjects.OfType<Node>().ToArray();

            foreach (var visibilityNode in _GetVisibilityNodes(ownedNodes))
            {
                result[visibilityNode.Index] = true;
            }

            return result;
        }

        private IEnumerable<Node> _GetVisibilityNodes(Node[] ownedNodes)
        {
            var closedNodes = new HashSet<Node>();
            var priorityQueue = new PriorityQueue<Node, int>(0);
            foreach (var ownedNode in ownedNodes)
                priorityQueue.Enqueue(ownedNode, ownedNode.Visibility);

            while (priorityQueue.Count != 0)
            {
                var (node, currentVisibility) = priorityQueue.Dequeue();
                closedNodes.Add(node);
                yield return node;
                if (currentVisibility == 0) continue;
                foreach (var neighbor in node.GetNeighbors())
                {
                    if (closedNodes.Contains(neighbor))
                        continue;
                    var nextVisibility = currentVisibility - 1 - neighbor.ValueOfHidden;
                    priorityQueue.Enqueue(neighbor, nextVisibility);
                }
            }
        }
    }
}