using System;
using System.Diagnostics.CodeAnalysis;


namespace LineWars.Model
{
    public interface IBuildAction<TNode, TEdge, TUnit> :
        IUnitAction<TNode, TEdge, TUnit>,
        ITargetedAction<TEdge>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        bool CanUpRoad([NotNull] TEdge edge, bool ignoreActionPointsCondition = false);
        void UpRoad([NotNull] TEdge edge);

        bool ITargetedAction<TEdge>.IsAvailable(TEdge target) => CanUpRoad(target);
        void ITargetedAction<TEdge>.Execute(TEdge target) => UpRoad(target);

        IActionCommand ITargetedAction<TEdge>.GenerateCommand(TEdge target)
        {
            return new BuildCommand<TNode, TEdge, TUnit>(this, target);
        }
    }
}