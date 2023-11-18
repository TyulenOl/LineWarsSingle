using System;
using System.Collections;
using System.Linq;

namespace LineWars.Model
{
    public class RamAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IRamAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public int Damage { get; }

        public override CommandType CommandType => CommandType.Ram;
        public override ActionType ActionType => ActionType.Targeted;

        public RamAction(TUnit executor, int damage) : base(executor)
        {
            Damage = damage;
        }

        public bool CanRam(TNode node)
        {
            var line = MyUnit.Node.GetLine(node);
            return ActionPointsCondition()
                   && line != null
                   && MyUnit.CanMoveOnLineWithType(line.LineType)
                   && node.OwnerId != MyUnit.OwnerId
                   && !node.AllIsFree;
        }

        public void Ram(TNode node)
        {
            var enumerator = SlowRam(node);
            while (enumerator.MoveNext())
            {
            }
        }

        public IEnumerator SlowRam(TNode enemyNode)
        {
            var enemies = enemyNode.Units
                .ToArray();
            var damage = Damage / enemies.Length;
            var possibleNodeForRetreat = enemyNode
                .GetNeighbors()
                .Where(node => node.OwnerId == enemyNode.OwnerId)
                .ToArray();

            foreach (var enemy in enemies)
            {
                if (enemy.CurrentHp + enemy.CurrentArmor <= damage)
                {
                    enemy.CurrentHp = 0;
                    yield return new DiedUnit(enemy);
                    continue;
                }

                var nodeForRetreat = possibleNodeForRetreat
                    .FirstOrDefault(x => UnitUtilities<TNode, TEdge, TUnit>.CanMoveTo(enemy, x));
                if (nodeForRetreat == null)
                {
                    enemy.CurrentHp = 0;
                    yield return new DiedUnit(enemy);
                    continue;
                }

                enemy.DealDamageThroughArmor(damage);
                UnitUtilities<TNode, TEdge, TUnit>.MoveTo(enemy, nodeForRetreat);
                yield return new MovedUnit(enemy, nodeForRetreat);
            }

            UnitUtilities<TNode, TEdge, TUnit>.MoveTo(MyUnit, enemyNode);
            yield return new MovedUnit(MyUnit, enemyNode);
            CompleteAndAutoModify();
        }

        public override void Accept(IBaseUnitActionVisitor<TNode, TEdge, TUnit> visitor)
        {
            visitor.Visit(this);
        }

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor)
        {
            return visitor.Visit(this);
        }
    }
}