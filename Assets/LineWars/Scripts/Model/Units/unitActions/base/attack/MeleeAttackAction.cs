using System;
using System.Diagnostics.CodeAnalysis;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public sealed class MeleeAttackAction<TNode, TEdge, TUnit> :
            AttackAction<TNode, TEdge, TUnit>,
            IMeleeAttackAction<TNode, TEdge, TUnit>

        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public UnitBlockerSelector BlockerSelector { get; }
        public bool Onslaught { get; }
        
        private readonly IMoveAction<TNode, TEdge, TUnit> moveAction;
        
        public override CommandType CommandType => CommandType.MeleeAttack;

        public override bool CanAttackFrom([NotNull] TNode node, [NotNull] TUnit enemy,
            bool ignoreActionPointsCondition = false)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));

            var line = node.GetLine(enemy.Node);
            return !AttackLocked
                   && Damage > 0
                   && enemy.OwnerId != MyUnit.OwnerId
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
                var selectedUnit = BlockerSelector.SelectBlocker<TNode, TEdge, TUnit>(enemy, neighbour);
                AttackUnitButIgnoreBlock(selectedUnit);
                if (selectedUnit != enemy)
                {
                    //Debug.Log($"Юнит {selectedUnit} перехватил атаку");
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
                && Onslaught
                && moveAction.CanMoveTo(enemyNode, true)
               )
            {
                moveAction.MoveTo(enemyNode);
            }
        }

        private void MeleeAttack(TUnit target)
        {
            var enemyDamage = target.GetMaxDamage<TNode, TEdge, TUnit>();
            target.DealDamageThroughArmor(Damage);
            if(!target.IsDied)
                MyUnit.DealDamageThroughArmor(enemyDamage/2);
            CompleteAndAutoModify();
        }
        
        public MeleeAttackAction(
            TUnit executor,
            int damage,
            bool isPenetrating,
            bool onslaught,
            [NotNull] UnitBlockerSelector blockerSelector) : base(executor, damage, isPenetrating)
        {
            if(blockerSelector == null) throw new ArgumentException();
            Onslaught = onslaught;
            if (blockerSelector == null)
                throw new ArgumentException($"blocker selector is null");
            BlockerSelector = blockerSelector;
            moveAction = executor.GetUnitAction<IMoveAction<TNode, TEdge, TUnit>>();
        }
        
        public override void Accept(IBaseUnitActionVisitor<TNode, TEdge, TUnit> visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor) => visitor.Visit(this);
    }
}