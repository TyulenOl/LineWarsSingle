using System;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New DistanceAttack", menuName = "UnitActions/Attack/DistanceAttack", order = 61)]
    public class DistanceUnitAttackActionData: BaseUnitAttackActionData
    {
        [field: SerializeField, Min(0)] public int Distance { get; private set; }
        public override ComponentUnit.UnitAction GetAction(ComponentUnit unit) => new ComponentUnit.DistanceAttackAction(unit, this);
    }
    
    public sealed partial class ComponentUnit
    {
        public class DistanceAttackAction: BaseAttackAction
        {
            public uint Distance { get; private set; }
            public DistanceAttackAction([NotNull] ComponentUnit unit, DistanceUnitAttackActionData data) : base(unit, data)
            {
                Distance = (uint) data.Distance;
            }
            
            public override bool CanAttackForm(Node node, ComponentUnit unit, bool ignoreActionPointsCondition = false)
            {
                return !AttackLocked
                       && Damage > 0
                       && unit.Owner != MyUnit.Owner
                       && node.FindShortestPath(unit.Node).Count - 1 <= Distance
                       && (ignoreActionPointsCondition || ActionPointsCondition());
            }

            public override void Attack(ComponentUnit enemy)
            {
                DistanceAttack(enemy, Damage);
                SfxManager.Instance.Play(ActionSfx);
                
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