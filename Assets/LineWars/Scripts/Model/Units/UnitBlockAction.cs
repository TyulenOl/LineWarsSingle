using System;
using JetBrains.Annotations;
using UnityEngine;

namespace LineWars.Model
{
    //[CreateAssetMenu(fileName = "New ContrAttackAction", menuName = "UnitActions/ContrAttackAction", order = 61)]
    [RequireComponent(typeof(BaseUnitAttackAction))]
    public class UnitBlockAction : BaseUnitAction
    {
        [SerializeField] private bool protection = false;
        [SerializeField] private IntModifier contrAttackDamageModifier;
        public bool Protection => protection;
        public IntModifier ContrAttackDamageModifier => contrAttackDamageModifier;

        public override ModelComponentUnit.UnitAction GetAction(ModelComponentUnit unit) =>
            new ModelComponentUnit.BlockAction(unit, this);
    }
    
    public sealed partial class ModelComponentUnit
    {
        public sealed class BlockAction : UnitAction, ISimpleAction
        {
            private bool isBlocked;
            private readonly IntModifier contrAttackDamageModifier;
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

            public BlockAction([NotNull] ModelComponentUnit unit, UnitBlockAction data) : base(unit, data)
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
            

            public bool CanContrAttack([NotNull] ModelComponentUnit enemy)
            {
                if (enemy == null) throw new ArgumentNullException(nameof(enemy));
                return (IsBlocked)
                    && contrAttackDamageModifier.Modify(AttackAction.Damage) > 0
                    && AttackAction.CanAttack(enemy, true);
            }

            public void ContrAttack([NotNull] ModelComponentUnit enemy)
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
            public ICommand GenerateCommand() => new EnableBlockCommand(this);
        }
    }
}