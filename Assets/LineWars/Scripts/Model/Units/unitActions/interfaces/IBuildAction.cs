using System;
using System.Diagnostics.CodeAnalysis;


namespace LineWars.Model
{
    public interface IBuildAction<TNode, TEdge, TUnit, TOwned, TPlayer> :
        IUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>,
        ITargetedAction
    
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer>
        #endregion 
    {
        bool CanUpRoad([NotNull] TEdge edge, bool ignoreActionPointsCondition = false);
        void UpRoad([NotNull] TEdge edge);
    }
}