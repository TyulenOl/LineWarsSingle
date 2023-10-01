using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [DisallowMultipleComponent]
    public abstract class MonoAttackAction : MonoUnitAction,
        IAttackAction<Node, Edge, Unit, Owned, BasePlayer, Nation>
    {
        protected AttackAction<Node, Edge, Unit, Owned, BasePlayer, Nation> AttackAction
            => (AttackAction<Node, Edge, Unit, Owned, BasePlayer, Nation>) ExecutorAction;

        [SerializeField] private int initialDamage;
        [SerializeField] private bool initialIsPenetratingDamage;
        public int InitialDamage => initialDamage;
        public bool InitialIsPenetratingDamage => initialIsPenetratingDamage;


        public int Damage => AttackAction.Damage;
        public bool IsPenetratingDamage => AttackAction.IsPenetratingDamage;
        public bool AttackLocked => AttackAction.AttackLocked;


        public bool CanAttack(IAlive enemy, bool ignoreActionPointsCondition = false) =>
            AttackAction.CanAttack(enemy, ignoreActionPointsCondition);

        public bool CanAttackFrom(Node node, IAlive enemy, bool ignoreActionPointsCondition = false) =>
            AttackAction.CanAttackFrom(node, enemy, ignoreActionPointsCondition);

        public bool CanAttackFrom(Node node, Unit enemy, bool ignoreActionPointsCondition = false) =>
            AttackAction.CanAttackFrom(node, enemy, ignoreActionPointsCondition);

        public bool CanAttackFrom(Node node, Edge edge, bool ignoreActionPointsCondition = false) =>
            AttackAction.CanAttackFrom(node, edge, ignoreActionPointsCondition);

        public void Attack(IAlive enemy) =>
            AttackAction.Attack(enemy);

        public void Attack(Unit unit) =>
            AttackAction.Attack(unit);

        public void Attack(Edge edge) => 
            AttackAction.Attack(edge);
    }
}