using UnityEngine;

namespace LineWars.Model
{
    public class Artillery: DistanceAttackUnit
    {
        [field: Header("Artillery Settings")]
        [SerializeField] private Explosion explosionPrefab;

        public override bool CanAttack(Edge edge) => CanAttack(edge, Node);

        public override bool CanAttack(Edge edge, Node node)
        {
            var pathLen1 = node.FindShortestPath(edge.FirstNode).Count - 1;
            var pathLen2 = node.FindShortestPath(edge.SecondNode).Count - 1;
            return !attackLocked 
                   && Damage > 0
                   && edge.LineType >= LineType.CountryRoad
                   && (pathLen1 <= Distance && pathLen2 + 1 <= Distance
                       || pathLen2 <= Distance && pathLen1 + 1 <= Distance)
                   && ActionPointsCondition(attackPointsModifier, CurrentActionPoints);
        }
        
        public override void Attack(Edge edge)
        {
            var explosion = Instantiate(explosionPrefab);
            explosion.transform.position = edge.transform.position;
            explosion.ExplosionEnded += () =>
            {
                DistanceAttack(edge);
            };
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