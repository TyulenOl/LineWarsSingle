namespace LineWars.Model
{
    public class ArtilleryAttackAction : DistanceAttackAction
    {
        public ArtilleryAttackAction(IUnit unit, ArtilleryUnitAttackAction data) : base(unit, data)
        {
        }

        public override bool CanAttackForm(INode node, IEdge edge, bool ignoreActionPointsCondition = false)
        {
            var pathLen1 = Graph.FindShortestPath(node, edge.FirstNode).Count - 1;
            var pathLen2 = Graph.FindShortestPath(node, edge.SecondNode).Count - 1;
            return !AttackLocked
                   && Damage > 0
                   && edge.LineType >= LineType.CountryRoad
                   && (pathLen1 <= Distance && pathLen2 + 1 <= Distance
                       || pathLen2 <= Distance && pathLen1 + 1 <= Distance)
                   && (ignoreActionPointsCondition || ActionPointsCondition());
        }


        public override void Attack(IEdge edge)
        {
            DistanceAttack(edge, Damage);
            CompleteAndAutoModify();
        }

        public override void Attack(IUnit enemy)
        {
            var damage = Damage;
            if (enemy.TryGetNeighbour(out var neighbour))
            {
                damage /= 2;
                DistanceAttack(neighbour, damage);
            }

            DistanceAttack(enemy, damage);
            CompleteAndAutoModify();
        }

        public override CommandType GetMyCommandType() => CommandType.Explosion;
    }
}