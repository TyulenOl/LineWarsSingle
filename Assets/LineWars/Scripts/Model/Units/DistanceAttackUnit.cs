using UnityEngine;

namespace LineWars.Model
{
    public class DistanceAttackUnit : Unit
    {
        [field: Header("Distance Attack Settings")]
        [field: SerializeField, Min(0)] public int Distance { get; private set; }

        public override bool CanAttack(Unit unit)
        {
            return !attackLocked
                   && Damage > 0
                   && unit.Owner != Owner
                   && Node.FindShortestPath(unit.Node).Count - 1 <= Distance
                   && ActionPointsCondition(attackPointsModifier, CurrentActionPoints);
        }

        public override void Attack(Unit enemy)
        {
            DistanceAttack(enemy);
        }

        protected void DistanceAttack(IAlive alive)
        {
            alive.TakeDamage(new Hit(Damage, this, alive, isPenetratingDamage, true));
            CurrentActionPoints = attackPointsModifier.Modify(CurrentActionPoints);
        
            ActionCompleted.Invoke();
        }

        public override CommandType GetAttackTypeBy(IAlive target)
        {
            return CommandType.Fire;
        }
    }
}