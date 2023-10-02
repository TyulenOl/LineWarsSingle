using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.PriorityQueue;

namespace LineWars.Model
{
    public class GraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> :
            Graph<TNode, TEdge>,
            IGraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
    
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TPlayer: class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TNation: class, INation<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        #endregion
    {
        public GraphForGame(IEnumerable<TNode> nodes, IEnumerable<TEdge> edges) : base(nodes, edges)
        {
        }
        
        public Dictionary<TNode, bool> GetVisibilityInfo(TPlayer player)
        {
            var result = new Dictionary<TNode, bool>(Nodes.Count());

            var ownedNodes = player.OwnedObjects.OfType<TNode>().ToArray();

            foreach (var node in Nodes)
                result[node] = false;
            foreach (var visibilityNode in _GetVisibilityNodes(ownedNodes))
                result[visibilityNode] = true;

            return result;
        }

        private IEnumerable<TNode> _GetVisibilityNodes(IReadOnlyCollection<TNode> ownedNodes)
        {
            if (ownedNodes == null || ownedNodes.Count == 0) throw new ArgumentException();
            if (ownedNodes.Any(x => !Nodes.Contains(x))) throw new InvalidOperationException();
            
            var closedNodes = new HashSet<TNode>();
            var priorityQueue = new PriorityQueue<TNode, int>(0);
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
    }
}