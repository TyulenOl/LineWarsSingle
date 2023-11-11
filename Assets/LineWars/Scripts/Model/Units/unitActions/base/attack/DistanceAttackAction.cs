using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class DistanceAttackAction<TNode, TEdge, TUnit> :
        AttackAction<TNode, TEdge, TUnit>,
        IDistanceAttackAction<TNode, TEdge, TUnit>
    
        #region Сonstraints
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit> 
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
        #endregion 
    {
        protected readonly IGraphForGame<TNode, TEdge, TUnit> Graph;
        public uint Distance { get; }

        public DistanceAttackAction(
            TUnit executor,
            int damage,
            bool isPenetratingDamage,
            uint distance,
            IGraphForGame<TNode, TEdge, TUnit> graph) : base(executor, damage, isPenetratingDamage)
        {
            Distance = distance;
            Graph = graph;
        }

        
        public override bool CanAttackFrom(TNode node, TUnit enemy, bool ignoreActionPointsCondition = false)
        {
            return !AttackLocked
                   && Damage > 0
                   && enemy.OwnerId != MyUnit.OwnerId
                   && Graph.FindShortestPath(node, enemy.Node).Count - 1 <= Distance
                   && (ignoreActionPointsCondition || ActionPointsCondition());
        }

        public override void Attack(TUnit enemy)
        {
            enemy.DealDamageThroughArmor(Damage);
            CompleteAndAutoModify();
        }
        
        public override uint GetPossibleMaxRadius() => Distance;

        public override CommandType CommandType => CommandType.Fire;
        
        public override void Accept(IBaseUnitActionVisitor<TNode, TEdge, TUnit> visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor) => visitor.Visit(this);
    }
}