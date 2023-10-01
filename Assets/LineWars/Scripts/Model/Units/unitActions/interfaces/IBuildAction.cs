using System;
using System.Diagnostics.CodeAnalysis;
using LineWars.Model.unitActions;

namespace LineWars.Model
{
    public interface IBuildAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> :
        IUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
    
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> 
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> 
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> 
        where TPlayer : class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TNation : class, INation<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
    {
        bool CanUpRoad([NotNull] TEdge edge, bool ignoreActionPointsCondition = false);

        bool CanUpRoad([NotNull] TEdge edge, [NotNull] TNode node, bool ignoreActionPointsCondition = false);

        void UpRoad([NotNull] TEdge edge);
    }
}