using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public interface IHealAction<TNode, TEdge, TUnit> : 
        IUnitAction<TNode, TEdge, TUnit>,
        ITargetedAction
    
        #region Сonstraints
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit> 
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
        #endregion 
    {
        bool IsMassHeal { get; }
        int HealingAmount { get; }
        bool CanHeal([NotNull] TUnit target, bool ignoreActionPointsCondition = false);
        void Heal([NotNull] TUnit target);
    }
}