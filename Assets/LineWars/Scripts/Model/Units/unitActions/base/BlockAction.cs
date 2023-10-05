using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace LineWars.Model
{
    public class BlockAction<TNode, TEdge, TUnit, TOwned, TPlayer> :
        UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>, 
        IBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer>
    
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer>
        #endregion 
    {
        private bool isBlocked;
        private readonly IntModifier contrAttackDamageModifier;
        private AttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> attackAction;

        public event Action<bool, bool> CanBlockChanged;

        private AttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> AttackAction
        {
            get
            {
                if (attackAction == null)
                {
                    if (MyUnit.TryGetUnitAction<AttackAction<TNode, TEdge, TUnit, TOwned, TPlayer>>(out var action)) 
                        attackAction = action;
                    else
                        throw new Exception("Для контратаки необходимо иметь хотябы атакующий компонент!");
                }
                return attackAction;
            }
        }

        public bool Protection { get; private set; }
        public bool IsBlocked => isBlocked || Protection;
        
        public BlockAction([NotNull] TUnit unit, [NotNull] MonoBlockAction data) : base (unit, data)
        {
            Protection = data.InitialProtection;
            contrAttackDamageModifier = data.InitialContrAttackDamageModifier;
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

        public bool CanContrAttack([NotNull] TUnit enemy)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            return (IsBlocked)
                && contrAttackDamageModifier.Modify(AttackAction.Damage) > 0
                && AttackAction.CanAttack(enemy, true);
        }

        public void ContrAttack([NotNull] TUnit enemy)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            var contrAttackDamage = contrAttackDamageModifier.Modify(AttackAction.Damage);

            enemy.CurrentHp -= contrAttackDamage;
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
