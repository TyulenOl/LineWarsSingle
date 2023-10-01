using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace LineWars.Model
{
    public class BlockAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> :
        UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>, 
        IBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
    
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TPlayer : class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TNation : class, INation<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
    {
        private bool isBlocked;
        private readonly IntModifier contrAttackDamageModifier;
        private AttackAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> attackAction;

        public event Action<bool, bool> CanBlockChanged;

        private AttackAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> AttackAction
        {
            get
            {
                if (attackAction == null)
                {
                    if (MyUnit.TryGetUnitAction<AttackAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>>(out var action)) 
                        attackAction = action;
                    else
                        throw new Exception("Для контратаки необходимо иметь хотябы атакующий компонент!");
                }
                return attackAction;
            }
        }

        public bool InitialProtection { get; private set; }
        public bool IsBlocked => isBlocked || InitialProtection;
        
        public BlockAction([NotNull] TUnit unit, [NotNull] MonoBlockAction data) : base (unit, data)
        {
            InitialProtection = data.InitialProtection;
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
