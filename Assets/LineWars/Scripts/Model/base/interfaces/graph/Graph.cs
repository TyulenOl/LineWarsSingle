using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DataStructures.PriorityQueue;

namespace LineWars.Model
{
    public class Graph<TNode, TEdge> : IGraph<TNode, TEdge>
        where TNode : class, INode<TNode, TEdge>
        where TEdge : class, IEdge<TNode, TEdge>
    {
        private readonly TNode[] nodes;
        private readonly TEdge[] edges;
        public IReadOnlyList<TNode> Nodes => nodes;
        public IReadOnlyList<TEdge> Edges => edges;

        public Graph(IEnumerable<TNode> nodes, IEnumerable<TEdge> edges)
        {
            this.nodes = nodes.ToArray();
            this.edges = edges.ToArray();
        }

        public List<TNode> FindShortestPath(
            [NotNull] TNode start,
            [NotNull] TNode end,
            Func<TNode, TNode, bool> condition = null)
        {
            if (start == null || !Nodes.Contains(start)) throw new ArgumentNullException(nameof(start));
            if (end == null || !Nodes.Contains(end)) throw new ArgumentNullException(nameof(end));

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
                    if (!Nodes.Contains(neighborhood)) throw new InvalidOperationException();
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
            if (startNode == null || !Nodes.Contains(startNode))
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
                    if (!Nodes.Contains(neighborhood)) throw new InvalidOperationException();
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
                throw new ArgumentException("Нет стартовых нод!");
            if (startNodes.Keys.Any(x => !Nodes.Contains(x))
                || interferingNodes.Keys.Any(x => !Nodes.Contains(x)))
                throw new InvalidOperationException("Есть ноды не содержащиеся в графе!");

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
    }
}