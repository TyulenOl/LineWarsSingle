using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [DisallowMultipleComponent]
    public abstract class MonoAttackAction : MonoUnitAction,
        IAttackAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        private AttackAction<Node, Edge, Unit, Owned, BasePlayer> AttackAction
            => (AttackAction<Node, Edge, Unit, Owned, BasePlayer>) ExecutorAction;

        [SerializeField] protected SFXData attackSfx;
        [field: SerializeField] public int InitialDamage { get; private set; }
        [field: SerializeField] public bool InitialIsPenetratingDamage { get; private set; }


        public int Damage => AttackAction.Damage;
        public bool IsPenetratingDamage => AttackAction.IsPenetratingDamage;
        

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
            return new AttackCommand<Node, Edge, Unit, Owned, BasePlayer>(this, (IAlive) target);
        }
    }
}