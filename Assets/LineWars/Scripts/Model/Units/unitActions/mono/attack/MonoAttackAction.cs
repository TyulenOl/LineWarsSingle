using System.Collections;
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

        private readonly IDJ DJ = new RandomDJ(1);
        [field: SerializeField] public bool InitialIsPenetratingDamage { get; private set; }

        public bool IsPenetratingDamage => Action.IsPenetratingDamage;
        

        public virtual bool CanAttack(ITargetedAlive enemy, bool ignoreActionPointsCondition = false) =>
            Action.CanAttack(enemy, ignoreActionPointsCondition);

        public virtual void Attack(ITargetedAlive enemy)
        {
            StartCoroutine(AttackCoroutine(enemy));
        }

        private IEnumerator AttackCoroutine(ITargetedAlive enemy)
        {
            if (attackSfx == null)
            {
                Debug.LogError($"{nameof(attackSfx)} is null on {Executor.gameObject.name}");
            }
            Action.Attack(enemy);
            Executor.PlaySfx(attackSfx);
            if(attackSfx != null && attackSfx.Clip != null)
                yield return new WaitForSeconds(attackSfx.LengthInSeconds / 2);
            Executor.PlaySfx(DJ.GetSound(sfxList));
        }
    }
}