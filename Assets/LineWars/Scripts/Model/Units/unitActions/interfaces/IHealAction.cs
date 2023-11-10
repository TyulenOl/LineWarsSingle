using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public interface IHealAction<TNode, TEdge, TUnit> : 
        IUnitAction<TNode, TEdge, TUnit>,
        ITargetedAction<TUnit>

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
        
        bool ITargetedAction<TUnit>.IsAvailable(TUnit target) => CanHeal(target);
        void ITargetedAction<TUnit>.Execute(TUnit target) => Heal(target);
        IActionCommand ITargetedAction<TUnit>.GenerateCommand(TUnit target)
        {
            return new HealCommand<TNode, TEdge, TUnit>(this, target);
        }
    }
}