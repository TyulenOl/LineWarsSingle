using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class Artillery: DistanceAttackUnit
    {
        [field: Header("ArtillerySettings")]
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
        
        public override CommandType GetAttackTypeBy(IAlive target)
        {
            return CommandType.Explosion;
        }
    }
}