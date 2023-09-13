using System;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New MeleeAttackAction", menuName = "UnitActions/Attack/MeleeAttack", order = 61)]
    public class MeleeAttackAction: BaseAttackAction
    {
        [SerializeField] private UnitBlockerSelector blockerSelector;
        /// <summary>
        /// указывет на то, нужно ли захватывать точку после атаки
        /// </summary>
        [SerializeField] private bool onslaught;
        
        public override CommandType GetMyCommandType() => CommandType.MeleeAttack;
        public override bool CanAttackForm([NotNull]Node node, [NotNull]CombinedUnit enemy, bool ignoreActionPointsCondition = false)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));

            var line = node.GetLine(enemy.Node);
            return !attackLocked
                   && Damage > 0
                   && enemy.Owner != MyUnit.Owner
                   && line != null
                   && MyUnit.CanMoveOnLineWithType(line.LineType)
                   && (ignoreActionPointsCondition || ActionPointsCondition());
        }

        public override void Attack([NotNull] CombinedUnit enemy)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));

            // если нет соседа, то тогда просто атаковать
            if (!enemy.TryGetNeighbour(out var neighbour))
                AttackUnitButIgnoreBlock(enemy);
            // иначе выбрать того, кто будет блокировать урон
            else
            {
                var selectedUnit = blockerSelector.SelectBlocker(enemy, neighbour);
                //if (selectedUnit == enemy)
                    //AttackUnitButIgnoreBlock(enemy);
                //else
                    //Invoker.ExecuteCommand(new BlockAttackCommand(MyUnit, selectedUnit));
            }
        }
        
        public void AttackUnitButIgnoreBlock([NotNull] CombinedUnit enemy)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            var enemyNode = enemy.Node;
            MeleeAttack(enemy);

            //if (enemy.IsDied && enemyNode.AllIsFree && onslaught) 
                //Invoker.ExecuteCommand(new MoveCommand(MyUnit, MyUnit.Node, enemyNode));
        }

        private void MeleeAttack(CombinedUnit target)
        {
            //target.TakeDamage(new Hit(Damage, MyUnit, target, isPenetratingDamage));
            MyUnit.CurrentActionPoints = ModifyActionPoints();
            SfxManager.Instance.Play(attackSfx);
            
            Complete();
        }
    }
}