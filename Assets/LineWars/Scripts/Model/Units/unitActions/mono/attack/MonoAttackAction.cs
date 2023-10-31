using System;
using System.Collections;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [DisallowMultipleComponent]
    public abstract class MonoAttackAction : MonoUnitAction<AttackAction<Node, Edge, Unit, Owned, BasePlayer>>,
        IAttackAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        private AttackAction<Node, Edge, Unit, Owned, BasePlayer> AttackAction
            => (AttackAction<Node, Edge, Unit, Owned, BasePlayer>) Action;

        [SerializeField] protected SFXData attackSfx;

        [SerializeField] protected SFXList sfxList;

        private IDJ DJ;
        
        [field: SerializeField] public int InitialDamage { get; private set; }
        [field: SerializeField] public bool InitialIsPenetratingDamage { get; private set; }


        public int Damage => AttackAction.Damage;
        public bool IsPenetratingDamage => AttackAction.IsPenetratingDamage;


        public override void Initialize()
        {
            base.Initialize();
            DJ = new RandomDJ(1);
        }

        public virtual bool CanAttack(IAlive enemy, bool ignoreActionPointsCondition = false) =>
            AttackAction.CanAttack(enemy, ignoreActionPointsCondition);

        public virtual void Attack(IAlive enemy)
        {
            StartCoroutine(AttackCoroutine(enemy));
        }

        private IEnumerator AttackCoroutine(IAlive enemy)
        {
            AttackAction.Attack(enemy);
            SfxManager.Instance.Play(attackSfx);
            yield return new WaitForSeconds(attackSfx.LengthInSeconds/2);
            SfxManager.Instance.Play(DJ.GetSound(sfxList));
        }

        public Type TargetType => typeof(IAlive);
        public bool IsMyTarget(ITarget target) => target is IAlive;
        public ICommandWithCommandType GenerateCommand(ITarget target)
        {
            return new AttackCommand<Node, Edge, Unit, Owned, BasePlayer>(this, (IAlive) target);
        }
        
    }
}