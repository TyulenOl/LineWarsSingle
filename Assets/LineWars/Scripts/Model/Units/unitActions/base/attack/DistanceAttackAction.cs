using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class DistanceAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> :
        AttackAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>,
        IDistanceAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
    
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TPlayer : class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TNation : class, INation<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
    {
        protected IGraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> Graph;
        public uint Distance { get; private set; }

        public DistanceAttackAction(TUnit unit,
            MonoDistanceAttackAction data,
            [NotNull] IGraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> graph) : base(unit, data)
        {
            this.Graph = graph ?? throw new ArgumentNullException(nameof(graph));
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
            DistanceAttack(enemy, Damage);
            CompleteAndAutoModify();
        }

        protected void DistanceAttack(IAlive enemy, int damage)
        {
            enemy.CurrentHp -= damage;
        }

        public override uint GetPossibleMaxRadius() => Distance;
        public override CommandType GetMyCommandType() => CommandType.Fire;
    }
}