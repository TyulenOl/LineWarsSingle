using System;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New MeleeAttackAction", menuName = "UnitActions/Attack/MeleeAttack", order = 61)]
    public class MeleeUnitAttackActionData : BaseUnitAttackActionData
    {
        [SerializeField] private UnitBlockerSelector blockerSelector;

        /// <summary>
        /// указывет на то, нужно ли захватывать точку после атаки
        /// </summary>
        [SerializeField] private bool onslaught = true;

        public UnitBlockerSelector BlockerSelector => blockerSelector;
        public bool Onslaught => onslaught;

        public override ComponentUnit.UnitAction GetAction(ComponentUnit unit) => new ComponentUnit.MeleeAttackAction(unit, this);
    }

    public sealed partial class ComponentUnit
    {
        public sealed class MeleeAttackAction : BaseAttackAction
        {
            private readonly UnitBlockerSelector blockerSelector;
            private readonly bool onslaught;

            public MeleeAttackAction([NotNull] ComponentUnit unit, MeleeUnitAttackActionData data) : base(unit, data)
            {
                blockerSelector = data.BlockerSelector;
                onslaught = data.Onslaught;
            }

            public override CommandType GetMyCommandType() => CommandType.MeleeAttack;

            public override bool CanAttackForm([NotNull] Node node, [NotNull] ComponentUnit enemy,
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

            public override void Attack([NotNull] ComponentUnit enemy)
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

            public void AttackUnitButIgnoreBlock([NotNull] ComponentUnit enemy)
            {
                if (enemy == null) throw new ArgumentNullException(nameof(enemy));
                var enemyNode = enemy.Node;
                MeleeAttack(enemy);

                if (enemy.IsDied && enemyNode.AllIsFree && onslaught)
                    UnitsController.ExecuteCommand(new UnitMoveCommand(MyUnit, MyUnit.Node, enemyNode));
            }

            private void MeleeAttack(ComponentUnit target)
            {
                target.TakeDamage(new Hit(Damage, MyUnit, target, IsPenetratingDamage));
                SfxManager.Instance.Play(ActionSfx);

                CompleteAndAutoModify();
            }
        }
    }
}