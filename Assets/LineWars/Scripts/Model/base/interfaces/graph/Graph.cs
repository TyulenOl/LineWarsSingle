using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace LineWars.Model
{
    public class Graph<TNode, TEdge> : IGraph<TNode, TEdge>
        where TNode : class, INode<TNode, TEdge>
        where TEdge : class, IEdge<TNode, TEdge>
    {
        private TNode[] nodes;
        private TEdge[] edges;
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
            [NotNull]TNode startNode,
            uint range,
            Func<TNode, TNode, bool> condition = null)
        {
            if (startNode == null || !Nodes.Contains(startNode)) throw new ArgumentException(nameof(startNode));
                
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
    }
}