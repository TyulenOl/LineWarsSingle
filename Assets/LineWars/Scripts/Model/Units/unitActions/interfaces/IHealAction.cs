using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public interface IHealAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> : 
        IUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>,
        ITargetedAction
    
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> 
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> 
        where TPlayer : class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TNation : class, INation<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
    {
        bool IsMassHeal { get; }
        int HealingAmount { get; }
        bool HealLocked { get; }
        bool CanHeal([NotNull] TUnit target, bool ignoreActionPointsCondition = false);
        void Heal([NotNull] TUnit target);
    }
}