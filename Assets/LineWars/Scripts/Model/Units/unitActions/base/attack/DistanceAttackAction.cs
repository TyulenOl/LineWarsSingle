using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class DistanceAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> :
        AttackAction<TNode, TEdge, TUnit, TOwned, TPlayer>,
        IDistanceAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer>
    
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TOwned, TPlayer>
        #endregion 
    {
        protected readonly IGraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer> Graph;
        public uint Distance { get; private set; }

        public DistanceAttackAction(TUnit unit,
            MonoDistanceAttackAction data,
            [NotNull] IGraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer> graph) : base(unit, data)
        {
            Graph = graph ?? throw new ArgumentNullException(nameof(graph));
        }

        public DistanceAttackAction(TUnit unit, DistanceAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> data,
            [NotNull] IGraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer> graph) : base(unit, data)
        {
            Graph = graph ?? throw new ArgumentNullException(nameof(graph));
        }

        public override bool CanAttackFrom(TNode node, TUnit enemy, bool ignoreActionPointsCondition = false)
        {
            return !AttackLocked
                   && Damage > 0
                   && enemy.Owner != MyUnit.Owner
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

        public override void Accept(IUnitActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> visitor)
        {
            visitor.Visit(this);
        }
    }
}