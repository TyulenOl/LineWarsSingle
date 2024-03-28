using System;

namespace LineWars.Model
{
    public interface IAttackAction
    {
        public int Damage { get; }
        public event Action<int> DamageChanged;
    }
    
    public interface IAttackAction<TNode, TEdge, TUnit> :
        IUnitAction<TNode, TEdge, TUnit>,
        ITargetedAction<ITargetedAlive>,
        IAttackAction
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        bool IsPenetratingDamage { get; }

        bool CanAttack(ITargetedAlive enemy, bool ignoreActionPointsCondition = false);
        void Attack(ITargetedAlive enemy);
        
        
        bool ITargetedAction<ITargetedAlive>.IsAvailable(ITargetedAlive target) => CanAttack(target);
        void ITargetedAction<ITargetedAlive>.Execute(ITargetedAlive target) => Attack(target);

        IActionCommand ITargetedAction<ITargetedAlive>.GenerateCommand(ITargetedAlive target)
        {
            return new AttackCommand<TNode, TEdge, TUnit>(this, target);
        }
    }
}