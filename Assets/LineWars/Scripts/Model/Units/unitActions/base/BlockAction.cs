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
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TOwned, TPlayer>
        #endregion 
    {
        private bool isBlocked;
        public IntModifier ContrAttackDamageModifier { get; set; }
        private IAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> attackAction;

        public event Action<bool, bool> CanBlockChanged;
        
        public bool Protection { get;  set; }
        public bool IsBlocked => isBlocked || Protection;
        
        
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
                && ContrAttackDamageModifier.Modify(attackAction.Damage) > 0
                && attackAction.CanAttack(enemy, true);
        }

        public void ContrAttack([NotNull] TUnit enemy)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            var contrAttackDamage = ContrAttackDamageModifier.Modify(attackAction.Damage);

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

        public override CommandType CommandType => CommandType.Block;

        public ICommandWithCommandType GenerateCommand()
        {
            return new BlockCommand<TNode, TEdge, TUnit, TOwned, TPlayer>(this);
        }

        public override void Accept(IUnitActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> visitor)
        {
            visitor.Visit(this);
        }

        public BlockAction(
            TUnit executor,
            IntModifier contrAttackDamageModifier,
            bool protection) : base(executor)
        {
            ContrAttackDamageModifier = contrAttackDamageModifier;
            Protection = protection;
            attackAction = MyUnit.GetUnitAction<IAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer>>();
        }
    }
}
