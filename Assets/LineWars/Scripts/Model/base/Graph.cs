using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DataStructures.PriorityQueue;
using UnityEngine;

namespace LineWars.Model
{
    public class Graph<TNode, TEdge> : IGraph<TNode, TEdge>,
        INodeIndexer<TNode, TEdge>,
        IEdgeIndexer<TNode, TEdge>
        where TNode : class, INode<TNode, TEdge>
        where TEdge : class, IEdge<TNode, TEdge>
    {
        private readonly List<TNode> nodes;
        private readonly List<TEdge> edges;

        private readonly Dictionary<int, TNode> idToNode;
        private readonly Dictionary<int, TEdge> idToEdge;

        public INodeIndexer<TNode, TEdge> IdToNode => this;
        public IEdgeIndexer<TNode, TEdge> IdToEdge => this;
        public IReadOnlyList<TNode> Nodes => nodes;
        public IReadOnlyList<TEdge> Edges => edges;
        

        public Graph()
        {
            nodes = new List<TNode>();
            edges = new List<TEdge>();

            idToNode = new Dictionary<int, TNode>();
            idToEdge = new Dictionary<int, TEdge>();
        }
        
        public Graph(IEnumerable<TNode> nodes, IEnumerable<TEdge> edges)
        {
            this.nodes = nodes.ToList();
            this.edges = edges.ToList();

            idToNode = this.nodes.ToDictionary(x => x.Id, x => x);
            idToEdge = this.edges.ToDictionary(x => x.Id, x => x);
        }
        
        public void AddNode(TNode node)
        {
            if (node == null)
                throw new ArgumentNullException();
            if (idToNode.ContainsKey(node.Id))
                throw new ArgumentException();
            
            nodes.Add(node);
            idToNode.Add(node.Id, node);
        }

        public bool RemoveNode(TNode node)
        {
            return RemoveNode(node.Id);
        }

        public bool RemoveNode(int nodeId)
        {
            var removed = idToNode.Remove(nodeId, out var node);
            if (removed)
                nodes.Remove(node);
            
            return removed;
        }

        public bool ContainsNode(TNode node)
        {
            return idToNode.ContainsKey(node.Id);
        }
        
        public bool ContainsNode(int nodeId)
        {
            return idToNode.ContainsKey(nodeId);
        }

        public void AddEdge(TEdge edge)
        {
            if (edge == null)
                throw new ArgumentNullException();
            if (idToEdge.ContainsKey(edge.Id))
                throw new ArgumentException();
            if (edge.FirstNode == null 
                || edge.SecondNode == null
                || !idToNode.ContainsKey(edge.FirstNode.Id)
                || !idToNode.ContainsKey(edge.SecondNode.Id))
                throw new ArgumentException();
            
            edges.Add(edge);
            idToEdge.Add(edge.Id, edge);
        }

        public bool RemoveEdge(TEdge edge)
        {
            return RemoveEdge(edge.Id);
        }

        public bool RemoveEdge(int edgeId)
        {
            var removed = idToEdge.Remove(edgeId, out var edge);
            if (removed)
                edges.Remove(edge);
            
            return removed;
        }

        public bool ContainsEdge(TEdge edge)
        {
            return idToEdge.ContainsKey(edge.Id);
        }

        public bool ContainsEdge(int edgeId)
        {
            return idToEdge.ContainsKey(edgeId);
        }

        TNode INodeIndexer<TNode, TEdge>.this[int id]
        {
            get => idToNode[id];
            set
            {
                if (idToNode.Remove(id, out var node))
                    nodes.Remove(node);
                AddNode(value);
            }
        }

        TEdge IEdgeIndexer<TNode, TEdge>.this[int id]
        {
            get => idToEdge[id];
            set
            {
                if (idToEdge.Remove(id, out var edge))
                    edges.Remove(edge);
                AddEdge(value);
            }
        }


        public List<TNode> FindShortestPath(
            [NotNull] TNode start,
            [NotNull] TNode end,
            Func<TNode, TNode, bool> condition = null)
        {
            if (start == null || !idToNode.ContainsKey(start.Id)) 
                throw new ArgumentNullException(nameof(start));
            if (end == null || !idToNode.ContainsKey(end.Id))
                throw new ArgumentNullException(nameof(end));

            var queue = new Queue<TNode>();
            var track = new Dictionary<TNode, TNode>();
            queue.Enqueue(start);
            track[start] = null;
            while (queue.Count != 0)
            {
                var node = queue.Dequeue();
                foreach (var neighborhood in node.GetNeighbors())
                {
                    if (track.ContainsKey(neighborhood)) continue;
                    if (!idToNode.ContainsKey(neighborhood.Id)) throw new InvalidOperationException();
                    if (condition != null && !condition(node, neighborhood)) continue;
                    track[neighborhood] = node;
                    queue.Enqueue(neighborhood);
                }

                if (track.ContainsKey(end)) break;
            }

            if (!track.ContainsKey(end))
                return new List<TNode>();

            var pathItem = end;
            var result = new List<TNode>();
            while (pathItem != null)
            {
                result.Add(pathItem);
                pathItem = track[pathItem];
            }

            result.Reverse();
            return result;
        }

        /// <summary>
        /// Действует как волновой алгоритм
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="range">Если 0, то возвращает только ноду, если 1, то возвращает ноду и ее соседей</param>
        /// <param name="condition">Условие перехода из ноды в ноду, если истина, то переход возможен, если ложно, то нет.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Стартовая нода не содержится в графе</exception>
        /// <exception cref="InvalidOperationException">Выход за пределы графа</exception>
        public IEnumerable<TNode> GetNodesInRange(
            [NotNull] TNode startNode,
            uint range,
            Func<TNode, TNode, bool> condition = null)
        {
            if (startNode == null || !idToNode.ContainsKey(startNode.Id))
                throw new ArgumentException(nameof(startNode));

            var queue = new Queue<TNode>();
            var distanceMemory = new Dictionary<TNode, uint>();

            queue.Enqueue(startNode);
            distanceMemory[startNode] = 0;
            while (queue.Count != 0)
            {
                var node = queue.Dequeue();
                yield return node;
                foreach (var neighborhood in node.GetNeighbors())
                {
                    if (distanceMemory.ContainsKey(neighborhood)) continue;
                    if (!idToNode.ContainsKey(neighborhood.Id)) throw new InvalidOperationException();
                    if (condition != null && !condition(node, neighborhood)) continue;
                    var distanceForNextNode = distanceMemory[node] + 1;
                    if (distanceForNextNode > range) continue;

                    distanceMemory[neighborhood] = distanceForNextNode;
                    queue.Enqueue(neighborhood);
                }
            }
        }

        /// <summary>
        /// Алгоритм обхода графа в ширину из нескольких точек,
        /// которые передаются в словаре startNodes, где по ключу ноды передается ее "сила".
        /// Сила - расстояние, на которое пройдет волновой алгоритм из данной точки.
        ///
        /// interferingNodes - ноды, которые мешают проходу, или могут ему помогать
        /// </summary>
        public IEnumerable<TNode> MultiStartsLimitedBfs(
            IReadOnlyDictionary<TNode, int> startNodes,
            IReadOnlyDictionary<TNode, int> interferingNodes)
        {
            if (startNodes.Count == 0)
            {
                Debug.LogError("Нет стартовых нод!");
                yield break;
            }

            if (startNodes.Keys.Any(x => !idToNode.ContainsKey(x.Id))
                || interferingNodes.Keys.Any(x => !idToNode.ContainsKey(x.Id)))
            {
                Debug.LogError("Есть ноды не содержащиеся в графе!");
                yield break;
            }

            var closedNodes = new HashSet<TNode>();
            var priorityQueue = new PriorityQueue<TNode, int>(0);
            foreach (var (node, value) in startNodes)
                priorityQueue.Enqueue(node, -value + FindValue(node));

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
                    var nextVisibility = currentVisibility + 1 + FindValue(neighbor);
                    if (nextVisibility > 0) continue;
                    priorityQueue.Enqueue(neighbor, nextVisibility);
                }
            }

            int FindValue(TNode node)
            {
                return interferingNodes.TryGetValue(node, out var negativeValue)
                    ? negativeValue
                    : 0;
            }
        }

        /// <summary>
        /// Возвращает ноду и расстояние до нее, расстояние в данном случае — это минимальное чисто нод,
        /// встретившихся по пути из rootNode, включая конечную.
        /// RootNode включена в перечисление и имеет расстояние 0.
        /// Не возвращает несвязанные части графа.
        /// </summary>
        public IEnumerable<(TNode, uint)> FindDistanceToNodes(TNode rootNode)
        {
            if (rootNode == null || !idToNode.ContainsKey(rootNode.Id))
                throw new ArgumentException(nameof(rootNode));
            
            var queue = new Queue<TNode>();
            var distanceMemory = new Dictionary<TNode, uint>();

            queue.Enqueue(rootNode);
            distanceMemory[rootNode] = 0;
            while (queue.Count != 0)
            {
                var node = queue.Dequeue();
                yield return (node, distanceMemory[node]);
                foreach (var neighborhood in node.GetNeighbors())
                {
                    if (distanceMemory.ContainsKey(neighborhood)) continue;
                    if (!idToNode.ContainsKey(neighborhood.Id)) throw new InvalidOperationException();
                    var distanceForNextNode = distanceMemory[node] + 1;
                    distanceMemory[neighborhood] = distanceForNextNode;
                    queue.Enqueue(neighborhood);
                }
            }
        }
        
        
        public HashSet<(TNode, TNode)> FindMostRemoteNodes()
        {
            var maxDistance = uint.MinValue;
            var mostRemoteNodes = new HashSet<(TNode, TNode)>();

            foreach (var node1 in nodes)
            {
                foreach (var (node2, distance) in FindDistanceToNodes(node1))
                {
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        mostRemoteNodes = new HashSet<(TNode, TNode)>() {(node1, node2)};
                    }
                    else if (distance == maxDistance 
                             && !mostRemoteNodes.Contains((node2, node1)))
                    {
                        mostRemoteNodes.Add((node1, node2));
                    }
                }
            }

            return mostRemoteNodes;
        }

        public static List<TNode> StaticFindShortestPath(
            [NotNull] TNode start,
            [NotNull] TNode end,
            Func<TNode, TNode, bool> condition = null)
        {
            var queue = new Queue<TNode>();
            var track = new Dictionary<TNode, TNode>();
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
                return new List<TNode>();

            var pathItem = end;
            var result = new List<TNode>();
            while (pathItem != null)
            {
                result.Add(pathItem);
                pathItem = track[pathItem];
            }

            result.Reverse();
            return result;
        }
    }
}