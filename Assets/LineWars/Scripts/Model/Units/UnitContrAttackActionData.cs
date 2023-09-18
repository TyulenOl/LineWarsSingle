using System;
using JetBrains.Annotations;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New ContrAttackAction", menuName = "UnitActions/ContrAttackAction", order = 61)]
    public class UnitContrAttackActionData : BaseUnitActionData
    {
        [SerializeField] private bool protection = false;
        [SerializeField] private IntModifier contrAttackDamageModifier;
        public bool Protection => protection;
        public IntModifier ContrAttackDamageModifier => contrAttackDamageModifier;

        public override ComponentUnit.UnitAction GetAction(ComponentUnit unit) =>
            new ComponentUnit.ContAttackAction(unit, this);
    }

    public sealed partial class ComponentUnit
    {
        public class ContAttackAction : UnitAction
        {
            private bool isBlocked;
            private IntModifier contrAttackDamageModifier;
            private BaseAttackAction attackAction;
            
            public bool Protection { get; private set; }
            public bool IsBlocked => isBlocked || Protection;

            public BaseAttackAction AttackAction {
                get
                {
                    if (attackAction == null)
                    {
                        if (MyUnit.TryGetExecutorAction<BaseAttackAction>(out var action)) 
                            attackAction = action;
                        else
                            Debug.LogError("Для контратаки необходимо иметь хотябы атакующий компонент!");
                    }
                    return attackAction;
                }
            }

            public event Action<bool, bool> CanBlockChanged;

            public ContAttackAction([NotNull] ComponentUnit unit, UnitContrAttackActionData data) : base(unit, data)
            {
                Protection = data.Protection;
                contrAttackDamageModifier = data.ContrAttackDamageModifier;
            }

            public bool CanBlock()
            {
                return ActionPointsCondition();
            }
            public void EnableBlock()
            {
                SetBlock(true);
                CompleteAndAutoModify();
            }
            

            public virtual bool CanContrAttack([NotNull] ComponentUnit enemy)
            {
                if (enemy == null) throw new ArgumentNullException(nameof(enemy));
                return (IsBlocked)
                    && contrAttackDamageModifier.Modify(AttackAction.Damage) > 0
                    && AttackAction.CanAttack(enemy, true);
            }

            public virtual void ContrAttack([NotNull] ComponentUnit enemy)
            {
                if (enemy == null) throw new ArgumentNullException(nameof(enemy));
                var contrAttackDamage = contrAttackDamageModifier.Modify(AttackAction.Damage);

                enemy.TakeDamage(new Hit(contrAttackDamage, MyUnit, enemy));
                SetBlock(false);
            }
            
            public override void OnReplenish()
            {
                base.OnReplenish();
                SetBlock(false);
            }

            private void SetBlock(bool value)
            {
                var before = isBlocked;
                isBlocked = value;
                if (before != value)
                    CanBlockChanged?.Invoke(before, value);
            }

            public override CommandType GetMyCommandType() => CommandType.Block;
        }
    }
}