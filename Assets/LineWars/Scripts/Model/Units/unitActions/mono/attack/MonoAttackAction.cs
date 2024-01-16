using System;
using System.Collections;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [DisallowMultipleComponent]
    public abstract class MonoAttackAction<TAction> :
        MonoUnitAction<TAction>,
        IAttackAction<Node, Edge, Unit>
        where TAction : AttackAction<Node, Edge, Unit>
    {
        [SerializeField] protected SFXData attackSfx;

        [SerializeField] protected SFXList sfxList;

        private IDJ DJ;
        [field: SerializeField] public bool InitialIsPenetratingDamage { get; private set; }

        public bool IsPenetratingDamage => Action.IsPenetratingDamage;


        public override void Initialize()
        {
            base.Initialize();
            DJ = new RandomDJ(1);
        }

        public virtual bool CanAttack(ITargetedAlive enemy, bool ignoreActionPointsCondition = false) =>
            Action.CanAttack(enemy, ignoreActionPointsCondition);

        public virtual void Attack(ITargetedAlive enemy)
        {
            StartCoroutine(AttackCoroutine(enemy));
        }

        private IEnumerator AttackCoroutine(ITargetedAlive enemy)
        {
            Action.Attack(enemy);
            Executor.PlaySfx(attackSfx);
            yield return new WaitForSeconds(attackSfx.LengthInSeconds / 2);
            Executor.PlaySfx(DJ.GetSound(sfxList));
        }
    }
}