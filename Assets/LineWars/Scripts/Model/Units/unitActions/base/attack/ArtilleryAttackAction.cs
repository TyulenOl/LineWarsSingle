namespace LineWars.Model
{
    public class ArtilleryAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> :
        DistanceAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer>,
        IArtilleryAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer>
    
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TOwned, TPlayer>
        #endregion 
    {
        public ArtilleryAttackAction(TUnit unit,
            MonoArtilleryAttackAction data, 
            IGraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer> graph) : base(unit, data, graph)
        {
     
        }
        public ArtilleryAttackAction(TUnit unit,
            ArtilleryAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> data,
            IGraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer> graph) : base(unit, data, graph)
        {

        }

        public override bool CanAttackFrom(TNode node, TEdge edge, bool ignoreActionPointsCondition = false)
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


        public override void Attack(TEdge edge)
        {
            DistanceAttack(edge, Damage);
            CompleteAndAutoModify();
        }

        public override void Attack(TUnit enemy)
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

        public override void Accept(IUnitActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> visitor)
        {
            visitor.Visit(this);
        }
    }
}