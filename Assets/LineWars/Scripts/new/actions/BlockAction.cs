using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace LineWars.Model
{
    public class BlockAction : UnitAction
    {
        private bool isBlocked;
        private readonly IntModifier contrAttackDamageModifier;
        private AttackAction attackAction;

        public event Action<bool, bool> CanBlockChanged;
        public AttackAction AttackAction
        {
            get
            {
                if (attackAction == null)
                {
                    if (MyUnit.TryGetExecutorAction<AttackAction>(out var action)) 
                        attackAction = action;
                    else
                        Debug.LogError("Для контратаки необходимо иметь хотябы атакующий компонент!");
                }
                return attackAction;
            }
        }

        public bool Protection { get; private set; }
        public bool IsBlocked => isBlocked || Protection;
        public BlockAction([NotNull] IUnit unit, [NotNull] MonoBlockAction data) : base (unit, data)
        {
            Protection = data.Protection;
            contrAttackDamageModifier = data.ContrAttackDamageModifier;
            unit.CurrentActionCompleted += (unitAction) =>
            {
                if (unitAction == this)
                    return;
                SetBlock(false);
            };
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

        public bool CanContrAttack([NotNull] IReadOnlyUnit enemy)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            return (IsBlocked)
                && contrAttackDamageModifier.Modify(AttackAction.Damage) > 0
                && AttackAction.CanAttack(enemy, true);
        }

        public void ContrAttack([NotNull] IUnit enemy)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            var contrAttackDamage = contrAttackDamageModifier.Modify(AttackAction.Damage);
        
            enemy.TakeDamage(new Hit(contrAttackDamage, MyUnit, enemy));
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
        public ICommand GenerateCommand() => new BlockCommand(this);

    }
}
