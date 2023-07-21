using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.PriorityQueue;
using UnityEngine;

namespace LineWars.Model
{
    public class Graph: MonoBehaviour
    {
        public static Graph Instance { get; private set; }
        private Node[] allNodes;
        private Edge[] allEdges;
        private SpawnInfo[] spawnInfos;

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

        public void Initialize(
            IEnumerable<Node> allNodes,
            IEnumerable<Edge> allEdges,
            IEnumerable<SpawnInfo> spawnInfos)
        {
            this.allNodes = allNodes.ToArray();
            this.allEdges = allEdges.ToArray();
            this.spawnInfos = spawnInfos.ToArray();
        }

        public IReadOnlyList<Node> AllNodes => allNodes;
        public IReadOnlyList<Edge> AllEdges => allEdges;
        public IReadOnlyList<SpawnInfo> SpawnInfos => spawnInfos;
        
        
        public bool[] GetVisibilityInfo(Player player)
        {
            var result = new bool[AllNodes.Count];

            var ownedNodes = player.OwnedObjects.OfType<Node>().ToArray();
            
            foreach (var visibilityNode in GetVisibilityNodes(ownedNodes))
            {
                result[visibilityNode.Index] = true;
            }

            return result;
        }

        private IEnumerable<Node> GetVisibilityNodes(Node[] ownedNodes)
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