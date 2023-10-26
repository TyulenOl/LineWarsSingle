using System;
using System.Diagnostics.CodeAnalysis;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public sealed class MeleeAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> :
            AttackAction<TNode, TEdge, TUnit, TOwned, TPlayer>,
            IMeleeAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer>

        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion

    {
        private readonly UnitBlockerSelector blockerSelector;
        private readonly bool onslaught;
        private readonly IMoveAction<TNode, TEdge, TUnit, TOwned, TPlayer> moveAction;
        
        public override CommandType CommandType => CommandType.MeleeAttack;

        public override bool CanAttackFrom([NotNull] TNode node, [NotNull] TUnit enemy,
            bool ignoreActionPointsCondition = false)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));

            var line = node.GetLine(enemy.Node);
            return !AttackLocked
                   && Damage > 0
                   && enemy.Owner != MyUnit.Owner
                   && line != null
                   && MyUnit.CanMoveOnLineWithType(line.LineType)
                   && (ignoreActionPointsCondition || ActionPointsCondition());
        }

        public override void Attack([NotNull] TUnit enemy)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));

            // если нет соседа, то тогда просто атаковать
            if (!enemy.TryGetNeighbour(out var neighbour))
                AttackUnitButIgnoreBlock(enemy);
            // иначе выбрать того, кто будет блокировать урон
            else
            {
                var selectedUnit =
                    blockerSelector.SelectBlocker<TNode, TEdge, TUnit, TOwned, TPlayer>(enemy, neighbour);
                AttackUnitButIgnoreBlock(selectedUnit);
                if (selectedUnit != enemy)
                {
                    Debug.Log($"Юнит {selectedUnit} перехватил атаку");
                }
            }
        }

        public void AttackUnitButIgnoreBlock([NotNull] TUnit enemy)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            var enemyNode = enemy.Node;
            MeleeAttack(enemy);

            if (enemy.IsDied
                && enemyNode.AllIsFree
                && onslaught
                && moveAction.CanMoveTo(enemyNode)
               )
            {
                moveAction.MoveTo(enemyNode);
            }
        }

        private void MeleeAttack(TUnit target)
        {
            var enemyDamage = target.GetMaxDamage<TNode, TEdge, TUnit, TOwned, TPlayer>();
            target.DealDamageThroughArmor(Damage);
            MyUnit.DealDamageThroughArmor(enemyDamage);
            CompleteAndAutoModify();
        }

        public override void Accept(IUnitActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> visitor)
        {
            visitor.Visit(this);
        }

        public MeleeAttackAction(TUnit executor) : base(executor)
        {
            moveAction = executor.GetUnitAction<IMoveAction<TNode, TEdge, TUnit, TOwned, TPlayer>>();
        }
    }
}