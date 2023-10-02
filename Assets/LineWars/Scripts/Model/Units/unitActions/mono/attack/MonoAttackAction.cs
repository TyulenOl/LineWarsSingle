using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [DisallowMultipleComponent]
    public abstract class MonoAttackAction : MonoUnitAction,
        IAttackAction<Node, Edge, Unit, Owned, BasePlayer, Nation>
    {
        private AttackAction<Node, Edge, Unit, Owned, BasePlayer, Nation> AttackAction
            => (AttackAction<Node, Edge, Unit, Owned, BasePlayer, Nation>) ExecutorAction;

        [SerializeField] protected SFXData attackSfx;
        [SerializeField] private int initialDamage;
        [SerializeField] private bool initialIsPenetratingDamage;
        public int InitialDamage => initialDamage;
        public bool InitialIsPenetratingDamage => initialIsPenetratingDamage;


        public int Damage => AttackAction.Damage;
        public bool IsPenetratingDamage => AttackAction.IsPenetratingDamage;
        public bool AttackLocked => AttackAction.AttackLocked;


        public virtual bool CanAttack(IAlive enemy, bool ignoreActionPointsCondition = false) =>
            AttackAction.CanAttack(enemy, ignoreActionPointsCondition);

        public virtual void Attack(IAlive enemy)
        {
            AttackAction.Attack(enemy);
            SfxManager.Instance.Play(attackSfx);
        }

        public bool IsMyTarget(ITarget target) => AttackAction.IsMyTarget(target);
        public ICommand GenerateCommand(ITarget target)
        {
            return new AttackCommand<Node, Edge, Unit, Owned, BasePlayer, Nation>(this, (IAlive) target);
        }
    }
}