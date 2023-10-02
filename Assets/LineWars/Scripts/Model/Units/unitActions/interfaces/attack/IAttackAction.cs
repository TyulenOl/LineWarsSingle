

namespace LineWars.Model
{
    public interface IAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> : 
        IUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>,
        ITargetedAction
    
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> 
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> 
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> 
        where TPlayer : class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TNation : class, INation<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
    {
        bool AttackLocked { get; }
        int Damage { get; }
        bool IsPenetratingDamage { get; }
        
        bool CanAttack(IAlive enemy, bool ignoreActionPointsCondition = false);
        void Attack(IAlive enemy);
    }
}