using System;
using System.Linq;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class ShotUnitAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IShotUnitAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public override CommandType CommandType => CommandType.ShotUnit;
        public TUnit TakenUnit { get; private set; }

        public ShotUnitAction(TUnit executor) : base(executor)
        {
        }

        public bool CanTakeUnit([NotNull] TUnit unit)
        {
            if (unit == null) throw new ArgumentNullException(nameof(unit));

            var line = MyUnit.Node.GetLine(unit.Node);
            return ActionPointsCondition()
                   && line != null;
        }

        public void TakeUnit([NotNull] TUnit unit)
        {
            TakenUnit = unit ?? throw new ArgumentNullException(nameof(unit));
        }

        public bool CanShotUnitTo(TNode node)
        {
            return TakenUnit != null;
        }

        public void ShotUnitTo(TNode node)
        {
            if (node.AllIsFree)
            {
                TakenUnit.Node = node;
            }
            else
            {
                var enemies = node.Units.ToArray();
                var myDamage = (TakenUnit.CurrentHp + TakenUnit.CurrentArmor) / enemies.Length;
                var enemyDamage = enemies.Select(x => x.CurrentHp + x.CurrentArmor).Sum();
                foreach (var enemy in enemies)
                    enemy.DealDamageThroughArmor(myDamage);
                TakenUnit.DealDamageThroughArmor(enemyDamage);
            }

            TakenUnit = null;
            CompleteAndAutoModify();
        }

        public Type TargetType { get; } = typeof(TUnit);
        public Type[] MyTargets { get; } = {typeof(TUnit), typeof(TNode)};

        public bool IsMyTarget(ITarget target)
        {
            return TakenUnit == null && target is TUnit || TakenUnit != null && target is TNode;
        }

        public ICommandWithCommandType GenerateCommand(ITarget target)
        {
            if (TakenUnit == null)
            {
                return new TakeUnitCommand<TNode, TEdge, TUnit>(this, (TUnit) target);
            }

            return new ShotUnitCommand<TNode, TEdge, TUnit>(this, (TNode) target);
        }


        public override void Accept(IUnitActionVisitor<TNode, TEdge, TUnit> visitor) => visitor.Visit(this);

        public override TResult Accept<TResult>(IIUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor) =>
            visitor.Visit(this);
    }
}