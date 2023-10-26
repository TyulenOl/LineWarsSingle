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
            edge.CurrentHp -= Damage;
            CompleteAndAutoModify();
        }

        public override void Attack(TUnit enemy)
        {
            var damage = Damage;
            if (enemy.TryGetNeighbour(out var neighbour))
            {
                damage /= 2;
                neighbour.CurrentHp -= damage;
            }

            enemy.CurrentHp -= damage;
            CompleteAndAutoModify();
        }

        public override CommandType CommandType => CommandType.Explosion;

        public override void Accept(IUnitActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> visitor)
        {
            visitor.Visit(this);
        }

        public ArtilleryAttackAction(TUnit executor, IGraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer> graph) : base(executor, graph)
        {
        }
    }
}