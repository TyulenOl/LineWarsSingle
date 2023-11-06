using System;
using System.Diagnostics.CodeAnalysis;


namespace LineWars.Model
{
    public interface IBuildAction<TNode, TEdge, TUnit> :
        IUnitAction<TNode, TEdge, TUnit>,
        ITargetedAction
    
        #region Сonstraints
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit> 
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
        #endregion 
    {
        bool CanUpRoad([NotNull] TEdge edge, bool ignoreActionPointsCondition = false);
        void UpRoad([NotNull] TEdge edge);
    }
}