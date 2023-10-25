

namespace LineWars.Model
{
    public interface IAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> :
        IActionWithDamage,
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
        bool IsPenetratingDamage { get; }
        
        bool CanAttack(IAlive enemy, bool ignoreActionPointsCondition = false);
        void Attack(IAlive enemy);
    }
}