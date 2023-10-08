using System.Diagnostics.CodeAnalysis;


namespace LineWars.Model
{
    public interface IMoveAction<TNode, TEdge, TUnit, TOwned, TPlayer>:
        IUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>,
        ITargetedAction
        
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TOwned, TPlayer>
        #endregion 
    {
        bool CanMoveTo([NotNull] TNode target, bool ignoreActionPointsCondition = false);
        void MoveTo([NotNull] TNode target);
    }
}