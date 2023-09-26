using System;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class DistanceUnitAttackAction: BaseUnitAttackAction
    {
        [field: SerializeField, Min(0)] public int Distance { get; private set; }
        public override ModelComponentUnit.UnitAction GetAction(ModelComponentUnit unit) => new ModelComponentUnit.DistanceAttackAction(unit, this);
    }
    
    public sealed partial class ModelComponentUnit
    {
        public class DistanceAttackAction: BaseAttackAction
        {
            public uint Distance { get; private set; }
            public DistanceAttackAction([NotNull] ModelComponentUnit unit, DistanceUnitAttackAction data) : base(unit, data)
            {
                Distance = (uint) data.Distance;
            }
            
            public override bool CanAttackForm(ModelNode node, ModelComponentUnit unit, bool ignoreActionPointsCondition = false)
            {
                return !AttackLocked
                       && Damage > 0
                       && unit.Owner != MyUnit.Owner
                       && node.FindShortestPath(unit.Node).Count - 1 <= Distance
                       && (ignoreActionPointsCondition || ActionPointsCondition());
            }

            public override void Attack(ModelComponentUnit enemy)
            {
                DistanceAttack(enemy, Damage);
                CompleteAndAutoModify();
            }

            protected void DistanceAttack(IAlive enemy, int damage)
            {
                enemy.TakeDamage(new Hit(damage, MyUnit, enemy, IsPenetratingDamage, true));
            }

            public override CommandType GetMyCommandType() => CommandType.Fire;
            public override uint GetPossibleMaxRadius() => Distance;
        }
    }
}