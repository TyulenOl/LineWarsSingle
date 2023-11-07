namespace LineWars.Model
{
    public interface IAttackAction<TNode, TEdge, TUnit> :
        IActionWithDamage,
        IUnitAction<TNode, TEdge, TUnit>,
        ITargetedAction<IAlive>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    
    {
        bool IsPenetratingDamage { get; }

        bool CanAttack(IAlive enemy, bool ignoreActionPointsCondition = false);
        void Attack(IAlive enemy);


        bool ITargetedAction<IAlive>.CanExecute(IAlive target) => CanAttack(target);
        void ITargetedAction<IAlive>.Execute(IAlive target) => Attack(target);

        IActionCommand ITargetedAction<IAlive>.GenerateCommand(IAlive target)
        {
            return new AttackCommand<TNode, TEdge, TUnit>(this, target);
        }
    }
}