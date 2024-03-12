using System;

namespace LineWars.Model
{
    public class HealingAttackAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IHealingAttackAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public override CommandType CommandType => CommandType.HealingAttack;
        public int Damage => Executor.CurrentPower;
        public event Action<int> DamageChanged;
        private void OnUnitPowerChanged(TUnit unit, int before, int after)
        {
            DamageChanged?.Invoke(after);
        }
        
        public HealingAttackAction(TUnit executor) : base(executor)
        {
            Executor.UnitPowerChanged += OnUnitPowerChanged;
        }

        ~HealingAttackAction()
        {
            Executor.UnitPowerChanged -= OnUnitPowerChanged;
        }

        public bool IsAvailable(TUnit target)
        {
            return ActionPointsCondition() &&
                target.OwnerId != Executor.OwnerId &&
                target != null &&
                Executor.Node.GetLine(target.Node) != null;
        }

        public void Execute(TUnit target)
        {
            Attack(target);
            CompleteAndAutoModify();
        }

        private void Attack(TUnit target)
        {
            var enemyNode = target.Node;
            target.DealDamageThroughArmor(Executor.CurrentPower);
            Executor.CurrentArmor += Executor.CurrentPower;
            if (target.IsDied
                && enemyNode.AllIsFree
                && UnitUtilities<TNode, TEdge, TUnit>.CanMoveTo(Executor, enemyNode)
               )
            {
                UnitUtilities<TNode, TEdge, TUnit>.MoveTo(Executor, enemyNode);
            }
        }

        public IActionCommand GenerateCommand(TUnit target)
        {
            return new TargetedUniversalCommand
                <TUnit, HealingAttackAction<TNode, TEdge, TUnit>, TUnit>
                (this, target);
        }

        public override void Accept(IBaseUnitActionVisitor<TNode, TEdge, TUnit> visitor)
        {
            visitor.Visit(this);
        }

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
