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

        public GraphForGame():base()
        {
            
        }
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
            var startNodes = ownedNodes.ToDictionary(x => x, x => x.Visibility);
            var hiddenNodes = Nodes
                .Where(x => !startNodes.ContainsKey(x))
                .ToDictionary(x => x, x => x.ValueOfHidden);
            return MultiStartsLimitedBfs(startNodes, hiddenNodes);
        }
    }
}