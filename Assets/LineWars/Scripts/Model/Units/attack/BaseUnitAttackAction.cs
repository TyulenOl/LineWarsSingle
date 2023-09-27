using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [DisallowMultipleComponent]
    public abstract class BaseUnitAttackAction: BaseUnitAction
    {
        [SerializeField] private int damage;
        [SerializeField] private bool isPenetratingDamage;
        public int Damage => damage;
        public bool IsPenetratingDamage => isPenetratingDamage;
    }


    public sealed partial class ModelComponentUnit
    {
        public abstract class BaseAttackAction: UnitAction, ITargetedAction
        {
            public bool AttackLocked { get; protected set; }
            public int Damage { get; protected set; }
            public bool IsPenetratingDamage { get; protected set; }

            protected BaseAttackAction([NotNull] ModelComponentUnit unit, BaseUnitAttackAction data) : base(unit, data)
            {
                Damage = data.Damage;
                IsPenetratingDamage = data.IsPenetratingDamage;
            }
            public bool CanAttack(IAlive enemy, bool ignoreActionPointsCondition = false) => CanAttackForm(MyUnit.Node, enemy, ignoreActionPointsCondition);
            public bool CanAttackForm(ModelNode node, IAlive enemy, bool ignoreActionPointsCondition = false)
            {
                return enemy switch
                {
                    ModelEdge edge => CanAttackForm(node, edge, ignoreActionPointsCondition),
                    ModelComponentUnit unit => CanAttackForm(node, unit, ignoreActionPointsCondition),
                    _ => false
                };
            }
    
            public void Attack(IAlive enemy)
            {
                switch (enemy)
                {
                    case ModelEdge edge:
                        Attack(edge);
                        break;
                    case ModelComponentUnit unit:
                        Attack(unit);
                        break;
                }
            }
            
    
            public virtual bool CanAttackForm(ModelNode node, ModelComponentUnit unit, bool ignoreActionPointsCondition = false) => false;
            public virtual bool CanAttackForm(ModelNode node, ModelEdge edge,  bool ignoreActionPointsCondition = false) => false;
            
            public virtual void Attack(ModelComponentUnit unit) {}
            public virtual void Attack(ModelEdge edge) {}

            public bool IsMyTarget(IReadOnlyTarget target) => target is IAlive;
            public ICommand GenerateCommand(IReadOnlyTarget target) => new UnitAttackCommand(this, (IAlive)target);
        }
    }
}