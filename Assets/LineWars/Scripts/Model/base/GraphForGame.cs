using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.PriorityQueue;

namespace LineWars.Model
{
    public class GraphForGame<TNode, TEdge, TUnit> :
        Graph<TNode, TEdge>,
        IGraphForGame<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public GraphForGame(IEnumerable<TNode> nodes, IEnumerable<TEdge> edges) : base(nodes, edges)
        {
        }

        public Dictionary<TNode, bool> GetVisibilityInfo(IEnumerable<TNode> ownedNodes)
        {
            var result = new Dictionary<TNode, bool>(Nodes.Count());
            foreach (var node in Nodes)
                result[node] = false;
            foreach (var visibilityNode in GetVisibilityNodes(ownedNodes))
                result[visibilityNode] = true;

            return result;
        }

        public IEnumerable<TNode> GetVisibilityNodes(IEnumerable<TNode> ownedNodes)
        {
            var startNodes = ownedNodes.ToArray();
            if (startNodes.Length == 0) throw new ArgumentException();
            if (startNodes.Any(x => !Nodes.Contains(x))) throw new InvalidOperationException();

            var closedNodes = new HashSet<TNode>();
            var priorityQueue = new PriorityQueue<TNode, int>(0);
            foreach (var ownedNode in startNodes)
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
    }
}