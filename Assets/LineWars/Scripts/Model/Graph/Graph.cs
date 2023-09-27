﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DataStructures.PriorityQueue;
using LineWars.Extensions;
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

        public IReadOnlyList<Node> Nodes => allNodes;
        public IReadOnlyList<Edge> Edges => allEdges;

        public static IReadOnlyList<Node> AllNodes => Instance != null ? Instance.allNodes : Array.Empty<Node>();
        public static IReadOnlyList<Edge> AllEdges => Instance != null ? Instance.allEdges : Array.Empty<Edge>();
        public static IReadOnlyList<SpawnInfo> Spawns => Instance != null ? Instance.spawnInfos : Array.Empty<SpawnInfo>();
        
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



        public static Dictionary<Node, bool> GetVisibilityInfo(BasePlayer player) =>
            Instance != null
                ? Instance._GetVisibilityInfo(player)
                : throw new NullReferenceException("Graph is not Instance!");

        private Dictionary<Node, bool> _GetVisibilityInfo(BasePlayer player)
        {
            var result = new Dictionary<Node, bool>(allNodes.Length);

            var ownedNodes = player.OwnedObjects.OfType<Node>().ToArray();

            foreach (var node in allNodes)
                result[node] = false;
            foreach (var visibilityNode in _GetVisibilityNodes(ownedNodes))
                result[visibilityNode] = true;

            return result;
        }

        private IEnumerable<Node> _GetVisibilityNodes(Node[] ownedNodes)
        {
            if (ownedNodes == null || ownedNodes.Length == 0)
                yield break;
            
            var closedNodes = new HashSet<Node>();
            var priorityQueue = new PriorityQueue<Node, int>(0);
            foreach (var ownedNode in ownedNodes)
                priorityQueue.Enqueue(ownedNode, -ownedNode.Visibility);

            while (priorityQueue.Count != 0)
            {
                var (node, currentVisibility) = priorityQueue.Dequeue();
                if (closedNodes.Contains(node)) continue;
                
                closedNodes.Add(node);
                yield return node;
                if (currentVisibility == 0) continue;
                foreach (var neighbor in node.GetNeighbors())
                {
                    if (closedNodes.Contains(neighbor)) continue;
                    var nextVisibility = currentVisibility + 1 + neighbor.ValueOfHidden;
                    if (nextVisibility > 0) continue;
                    priorityQueue.Enqueue(neighbor, nextVisibility);
                }
            }
        }

        public static List<INode> FindShortestPath([NotNull] INode start,
            [NotNull] INode end,
            Func<INode, INode, bool> condition = null)
        {
            if (start == null) throw new ArgumentNullException(nameof(start));
            if (end == null) throw new ArgumentNullException(nameof(end));

            var queue = new Queue<INode>();
            var track = new Dictionary<INode, INode>();
            queue.Enqueue(start);
            track[start] = null;
            while (queue.Count != 0)
            {
                var node = queue.Dequeue();
                foreach (var neighborhood in node.GetNeighbors())
                {
                    if (track.ContainsKey(neighborhood)) continue;
                    if (condition != null && !condition(node, neighborhood)) continue;
                    track[neighborhood] = node;
                    queue.Enqueue(neighborhood);
                }

                if (track.ContainsKey(end)) break;
            }

            if (!track.ContainsKey(end))
                return new List<INode>();
            
            var pathItem = end;
            var result = new List<INode>();
            while (pathItem != null)
            {
                result.Add(pathItem);
                pathItem = track[pathItem];
            }

            result.Reverse();
            return result;
        }
        
        public static IEnumerable<INode> GetNodesInRange(INode startNode, uint range)
        {
            var queue = new Queue<INode>();
            var distanceMemory = new Dictionary<INode, uint>();
            
            queue.Enqueue(startNode);
            distanceMemory[startNode] = 0;
            while (queue.Count != 0)
            {
                var node = queue.Dequeue();
                yield return node;
                foreach (var neighborhood in node.GetNeighbors())
                {
                    if (distanceMemory.ContainsKey(neighborhood)) continue;
                    var distanceForNextNode = distanceMemory[node] + 1;
                    if (distanceForNextNode >= range) continue;
                    
                    distanceMemory[neighborhood] = distanceForNextNode;
                    queue.Enqueue(neighborhood);
                }
            }
        }
    }
}