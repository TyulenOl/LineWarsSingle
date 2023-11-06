

namespace LineWars.Model
{
    public interface IAttackAction<TNode, TEdge, TUnit> :
        IActionWithDamage,
        IUnitAction<TNode, TEdge, TUnit>,
        ITargetedAction
    
        #region Сonstraints
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit> 
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
        #endregion 
    {
        bool IsPenetratingDamage { get; }
        
        bool CanAttack(IAlive enemy, bool ignoreActionPointsCondition = false);
        void Attack(IAlive enemy);
    }
}