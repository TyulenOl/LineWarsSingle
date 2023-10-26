using System;
using System.Collections;
using System.Linq;

namespace LineWars.Model
{
    public class RamAction<TNode, TEdge, TUnit, TOwned, TPlayer> :
            UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>,
            IRamAction<TNode, TEdge, TUnit, TOwned, TPlayer>

        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion

    {
        private readonly IMoveAction<TNode, TEdge, TUnit, TOwned, TPlayer> moveAction;

        public override CommandType CommandType => CommandType.Ram;

        public override void Accept(IUnitActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> visitor) =>
            visitor.Visit(this);

        public int Damage { get; }

        public bool CanRam(TNode node)
        {
            var line = MyUnit.Node.GetLine(node);
            return ActionPointsCondition()
                   && line != null
                   && MyUnit.CanMoveOnLineWithType(line.LineType)
                   && node.Owner != MyUnit.Owner
                   && !node.AllIsFree;
        }

        public void Ram(TNode node)
        {
            var enumerator = SlowRam(node);
            while (enumerator.MoveNext())
            {
            }
        }

        public IEnumerator SlowRam(TNode node)
        {
            var enemies = node.Units
                .ToArray();
            var enemyOwner = node.Owner;
            var possibleNodeForRetreat = node
                .GetNeighbors()
                .Where(x => x.Owner == enemyOwner)
                .ToArray();

            foreach (var enemy in enemies)
            {
                if (enemy.CurrentHp + enemy.CurrentArmor <= Damage)
                {
                    enemy.CurrentHp = 0;
                    yield return new DiedUnit() {Unit = enemy};
                    continue;
                }

                var enemyMoveAction = enemy.GetUnitAction<IMoveAction<TNode, TEdge, TUnit, TOwned, TPlayer>>();
                if (enemyMoveAction == null)
                {
                    enemy.CurrentHp = 0;
                    yield return new DiedUnit() {Unit = enemy};
                    continue;
                }

                var nodeForRetreat = possibleNodeForRetreat
                    .FirstOrDefault(x => enemyMoveAction.CanMoveTo(x));
                if (nodeForRetreat == null)
                {
                    enemy.CurrentHp = 0;
                    yield return new DiedUnit() {Unit = enemy};
                    continue;
                }

                moveAction.MoveTo(nodeForRetreat);
                yield return new MovedUnit() {Unit = enemy};
                enemy.DealDamageThroughArmor(Damage);
            }

            moveAction.MoveTo(node);
            CompleteAndAutoModify();
        }
        public Type TargetType => typeof(TNode);
        public bool IsMyTarget(ITarget target) => target is TNode;

        public ICommandWithCommandType GenerateCommand(ITarget target)
        {
            return new RamCommand<TNode, TEdge, TUnit, TOwned, TPlayer>(MyUnit, (TNode) target);
        }

        public RamAction(TUnit executor, int damage) : base(executor)
        {
            Damage = damage;
            moveAction = MyUnit.GetUnitAction<IMoveAction<TNode, TEdge, TUnit, TOwned, TPlayer>>();
        }
    }
}