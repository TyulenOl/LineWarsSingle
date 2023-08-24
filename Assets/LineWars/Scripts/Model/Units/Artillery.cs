using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class Artillery: Unit
    {
        [field: Header("ArtillerySettings")]
        [field: SerializeField, Min(0)] public int DistanceDamage { get; private set; }
        [field: SerializeField, Min(0)] public int Distance { get; private set; }
        [field: SerializeField] public bool IsPenetratingDistanceAttack { get; private set; }
        [SerializeField] private IntModifier distanceAttackPointsModifier;
        [SerializeField] private Explosion explosionPrefab;


        public override bool CanAttack(Edge edge)
        {
            return !attackLocked &&
                   edge.LineType >= LineType.CountryRoad
                   && Node.FindShortestPath(edge.FirstNode).Count - 1 <= Distance
                   && Node.FindShortestPath(edge.SecondNode).Count - 1 <= Distance;
        }

        public override void Attack(Edge edge)
        {
            DistanceAttack(edge);
        }

        public override bool CanAttack(Unit unit)
        {
            return !attackLocked
                   && unit.Owner != Owner
                   && Node.FindShortestPath(unit.Node).Count - 1 <= Distance;
        }

        public override void Attack(Unit enemy)
        {
            var explosion = Instantiate(explosionPrefab);
            explosion.transform.position = enemy.Node.Position;
            explosion.ExplosionEnded += () =>
            {
                if (enemy.TryGetNeighbour(out var neighbour)) 
                    DistanceAttack(neighbour);
                DistanceAttack(enemy);
            };
        }
        

        private void DistanceAttack(IAlive alive)
        {
            alive.TakeDamage(new Hit(DistanceDamage, this, alive, IsPenetratingDistanceAttack, true));
            CurrentActionPoints = distanceAttackPointsModifier
                ? distanceAttackPointsModifier.Modify(CurrentActionPoints)
                : 0;
        }

        public override CommandType GetAttackTypeBy(IAlive target)
        {
            return CommandType.Explosion;
        }
    }
}