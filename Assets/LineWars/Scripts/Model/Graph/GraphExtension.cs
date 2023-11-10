using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public static class GraphExtension
    {
        public static IEnumerable<TUnit> GetUnitsInRange<TNode, TEdge, TUnit>(
            this IGraphForGame<TNode, TEdge, TUnit> graphForGame, TNode starsNode, uint range)
            where TNode : class, INodeForGame<TNode, TEdge, TUnit>
            where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
            where TUnit : class, IUnit<TNode, TEdge, TUnit>
        {
            return graphForGame.GetNodesInRange(starsNode, range)
                .SelectMany(node => node.Units);
        }
    }
}