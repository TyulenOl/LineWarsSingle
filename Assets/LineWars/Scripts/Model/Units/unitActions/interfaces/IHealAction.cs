using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public interface IHealAction<TNode, TEdge, TUnit, TOwned, TPlayer> : 
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
        bool IsMassHeal { get; }
        int HealingAmount { get; }
        bool CanHeal([NotNull] TUnit target, bool ignoreActionPointsCondition = false);
        void Heal([NotNull] TUnit target);
    }
}