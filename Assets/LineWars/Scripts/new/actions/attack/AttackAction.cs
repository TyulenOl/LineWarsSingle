using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public abstract class AttackAction: UnitAction
    {
        public bool AttackLocked { get; protected set; }
        public int Damage { get; protected set; }
        public bool IsPenetratingDamage { get; protected set; }

        protected AttackAction([NotNull] IUnit unit, BaseUnitAttackAction data) : base(unit, data)
        {
            Damage = data.Damage;
            IsPenetratingDamage = data.IsPenetratingDamage;
        }
        public bool CanAttack(IAlive enemy, bool ignoreActionPointsCondition = false) => CanAttackForm(MyUnit.Node, enemy, ignoreActionPointsCondition);
        public bool CanAttackForm(INode node, IAlive enemy, bool ignoreActionPointsCondition = false)
        {
            return enemy switch
            {
                IEdge edge => CanAttackForm(node, edge, ignoreActionPointsCondition),
                IUnit unit => CanAttackForm(node, unit, ignoreActionPointsCondition),
                _ => false
            };
        }
    
        public void Attack(IAlive enemy)
        {
            switch (enemy)
            {
                case IEdge edge:
                    Attack(edge);
                    break;
                case IUnit unit:
                    Attack(unit);
                    break;
            }
        }
            
    
        public virtual bool CanAttackForm(INode node, IUnit enemy, bool ignoreActionPointsCondition = false) => false;
        public virtual bool CanAttackForm(INode node, IEdge edge,  bool ignoreActionPointsCondition = false) => false;
            
        public virtual void Attack(IUnit unit) {}
        public virtual void Attack(IEdge edge) {}

        public bool IsMyTarget(IReadOnlyTarget target) => target is IAlive;
        public ICommand GenerateCommand(IReadOnlyTarget target) => new AttackCommand(this, (IAlive)target);
    }
}