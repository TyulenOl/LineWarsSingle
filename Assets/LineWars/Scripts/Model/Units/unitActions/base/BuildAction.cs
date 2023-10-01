using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace LineWars.Model
{
    public class BuildAction <TNode, TEdge, TUnit, TOwned, TPlayer, TNation> :
        UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>, 
        IBuildAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> 
    
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TPlayer : class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TNation : class, INation<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
    {
        public BuildAction([NotNull] TUnit unit, [NotNull] MonoBuildRoadAction data) : base(unit, data)
        {
        }

        public bool CanUpRoad([NotNull] TEdge edge, bool ignoreActionPointsCondition = false)
            => CanUpRoad(edge, MyUnit.Node, ignoreActionPointsCondition);

        public bool CanUpRoad([NotNull] TEdge edge, [NotNull] TNode node,
            bool ignoreActionPointsCondition = false)
        {
            if (edge == null) throw new ArgumentNullException(nameof(edge));
            if (node == null) throw new ArgumentNullException(nameof(node));

            return node.Edges.Contains(edge)
                   && LineTypeHelper.CanUp(edge.LineType)
                   && (ignoreActionPointsCondition || ActionPointsCondition());
        }

        public void UpRoad([NotNull] TEdge edge)
        {
            if (edge == null) throw new ArgumentNullException(nameof(edge));
            edge.LineType = LineTypeHelper.Up(edge.LineType);

            CompleteAndAutoModify();
        }
        public override CommandType GetMyCommandType() => CommandType.Build;
    }
}