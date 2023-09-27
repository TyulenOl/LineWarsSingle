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


    public sealed partial class ComponentUnit
    {
        public abstract class BaseAttackAction: UnitAction, ITargetedAction
        {
            public bool AttackLocked { get; protected set; }
            public int Damage { get; protected set; }
            public bool IsPenetratingDamage { get; protected set; }

            protected BaseAttackAction([NotNull] ComponentUnit unit, BaseUnitAttackAction data) : base(unit, data)
            {
                Damage = data.Damage;
                IsPenetratingDamage = data.IsPenetratingDamage;
            }
            public bool CanAttack(IAlive enemy, bool ignoreActionPointsCondition = false) => CanAttackForm(MyUnit.Node, enemy, ignoreActionPointsCondition);
            public bool CanAttackForm(Node node, IAlive enemy, bool ignoreActionPointsCondition = false)
            {
                return enemy switch
                {
                    Edge edge => CanAttackForm(node, edge, ignoreActionPointsCondition),
                    ComponentUnit unit => CanAttackForm(node, unit, ignoreActionPointsCondition),
                    _ => false
                };
            }
    
            public void Attack(IAlive enemy)
            {
                switch (enemy)
                {
                    case Edge edge:
                        Attack(edge);
                        break;
                    case ComponentUnit unit:
                        Attack(unit);
                        break;
                }
            }
            
    
            public virtual bool CanAttackForm(Node node, ComponentUnit unit, bool ignoreActionPointsCondition = false) => false;
            public virtual bool CanAttackForm(Node node, Edge edge,  bool ignoreActionPointsCondition = false) => false;
            
            public virtual void Attack(ComponentUnit unit) {}
            public virtual void Attack(Edge edge) {}

            public bool IsMyTarget(IReadOnlyTarget target) => target is IAlive;
            public ICommand GenerateCommand(IReadOnlyTarget target) => new UnitAttackCommand(this, (IAlive)target);
        }
    }
}