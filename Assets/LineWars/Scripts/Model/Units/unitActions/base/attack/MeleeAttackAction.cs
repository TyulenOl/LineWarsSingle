using System;
using System.Diagnostics.CodeAnalysis;
using LineWars.Controllers;

namespace LineWars.Model
{
    public sealed class MeleeAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> :
        AttackAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>,
        IMeleeAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
    
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TPlayer : class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TNation : class, INation<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
    {
        private readonly UnitBlockerSelector blockerSelector;
        private readonly bool onslaught;

        public MeleeAttackAction([NotNull] TUnit unit, MonoMeleeAttackAction data) : base(unit, data)
        {
            blockerSelector = data.InitialBlockerSelector;
            onslaught = data.InitialOnslaught;
        }

        public override CommandType GetMyCommandType() => CommandType.MeleeAttack;

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
                var selectedUnit = blockerSelector.SelectBlocker<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>(enemy, neighbour);
                if (selectedUnit == enemy)
                    AttackUnitButIgnoreBlock(enemy);
                else
                    UnitsController.ExecuteCommand(new BlockAttackCommand<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>(MyUnit, selectedUnit), false);
            }
        }

        public void AttackUnitButIgnoreBlock([NotNull] TUnit enemy)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            var enemyNode = enemy.Node;
            MeleeAttack(enemy);

            if (enemy.IsDied && enemyNode.AllIsFree && onslaught)
                UnitsController.ExecuteCommand(new MoveCommand<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>(MyUnit, enemyNode));
        }

        private void MeleeAttack(TUnit target)
        {
            target.CurrentHp -= Damage;
            CompleteAndAutoModify();
        }
    }
}