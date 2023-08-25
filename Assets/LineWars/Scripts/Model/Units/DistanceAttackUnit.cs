using UnityEngine;

namespace LineWars.Model
{
    public class DistanceAttackUnit : Unit
    {
        [field: Header("DistanceAttackSettings")]
        [field: SerializeField, Min(0)] public int DistanceDamage { get; private set; }
        [field: SerializeField, Min(0)] public int Distance { get; private set; }
        [field: SerializeField] public bool IsPenetratingDistanceAttack { get; private set; }
        [SerializeField] private IntModifier distanceAttackPointsModifier;

        public override bool CanAttack(Unit unit)
        {
            return !attackLocked
                   && unit.Owner != Owner
                   && Node.FindShortestPath(unit.Node).Count - 1 <= Distance;
        }

        public override void Attack(Unit enemy)
        {
            if (enemy.TryGetNeighbour(out var neighbour))
                DistanceAttack(neighbour);
            DistanceAttack(enemy);
        }

        protected void DistanceAttack(IAlive alive)
        {
            alive.TakeDamage(new Hit(DistanceDamage, this, alive, IsPenetratingDistanceAttack, true));
            CurrentActionPoints = distanceAttackPointsModifier
                ? distanceAttackPointsModifier.Modify(CurrentActionPoints)
                : 0;
            ActionCompleted.Invoke();
        }

        public override CommandType GetAttackTypeBy(IAlive target)
        {
            return CommandType.Fire;
        }
    }
}