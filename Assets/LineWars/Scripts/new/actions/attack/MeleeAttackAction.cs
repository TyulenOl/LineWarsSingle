using System;
using System.Diagnostics.CodeAnalysis;
using LineWars.Controllers;

namespace LineWars.Model
{
    public sealed class MeleeAttackAction : AttackAction
    {
        private readonly UnitBlockerSelector blockerSelector;
        private readonly bool onslaught;

        public MeleeAttackAction([NotNull] IUnit unit, MonoMeleeAttackAction data) : base(unit, data)
        {
            blockerSelector = data.BlockerSelector;
            onslaught = data.Onslaught;
        }

        public override CommandType GetMyCommandType() => CommandType.MeleeAttack;

        public override bool CanAttackForm([NotNull] INode node, [NotNull] IUnit enemy,
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

        public override void Attack([NotNull] IUnit enemy)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));

            // если нет соседа, то тогда просто атаковать
            if (!enemy.TryGetNeighbour(out var neighbour))
                AttackUnitButIgnoreBlock(enemy);
            // иначе выбрать того, кто будет блокировать урон
            else
            {
                var selectedUnit = blockerSelector.SelectBlocker(enemy, neighbour);
                if (selectedUnit == enemy)
                    AttackUnitButIgnoreBlock(enemy);
                else
                    UnitsController.ExecuteCommand(new BlockAttackCommand(MyUnit, selectedUnit), false);
            }
        }

        public void AttackUnitButIgnoreBlock([NotNull] IUnit enemy)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            var enemyNode = enemy.Node;
            MeleeAttack(enemy);

            if (enemy.IsDied && enemyNode.AllIsFree && onslaught)
                UnitsController.ExecuteCommand(new MoveCommand(MyUnit, enemyNode));
        }

        private void MeleeAttack(IUnit target)
        {
            target.TakeDamage(new Hit(Damage, MyUnit, target, IsPenetratingDamage));
            CompleteAndAutoModify();
        }
    }
}