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
        public override ActionType ActionType => ActionType.MultiTargeted;


        public ShotUnitAction(TUnit executor) : base(executor)
        {
        }

        public bool IsAvailable([NotNull] TUnit unit)
        {
            if (unit == null) throw new ArgumentNullException(nameof(unit));

            var line = MyUnit.Node.GetLine(unit.Node);
            return ActionPointsCondition()
                   && unit.Size == UnitSize.Little
                   && (MyUnit.IsNeighbour(unit) || line != null)
                   && MyUnit != unit;
        }

        public bool IsAvailable(TUnit target1, TNode target2)
        {
            return IsAvailable(target1)
                   && target2 != MyUnit.Node
                   && target2 != target1.Node;
        }

        public void Execute(TUnit takenUnit, TNode node)
        {
            if (takenUnit.Node.LeftUnit == takenUnit)
                takenUnit.Node.LeftUnit = null;
            if (takenUnit.Node.RightUnit == takenUnit)
                takenUnit.Node.RightUnit = null;

            if (node.AllIsFree)
            {
                AssignNode(takenUnit, node);
            }
            else
            {
                var enemies = node.Units.ToArray();
                var myDamage = (takenUnit.CurrentHp + takenUnit.CurrentArmor) / enemies.Length;
                var enemyDamage = enemies.Select(x => x.CurrentHp + x.CurrentArmor).Sum();
                foreach (var enemy in enemies)
                    enemy.DealDamageThroughArmor(myDamage);
                takenUnit.DealDamageThroughArmor(enemyDamage);
                if (!takenUnit.IsDied)
                {
                    AssignNode(takenUnit, node);
                }
            }


            CompleteAndAutoModify();
        }

        private static void AssignNode(TUnit takenUnit, TNode node)
        {
            takenUnit.Node = node;
            takenUnit.UnitDirection = UnitDirection.Left;
            node.LeftUnit = takenUnit;
            node.ConnectTo(takenUnit.OwnerId);
        }


        public override void Accept(IBaseUnitActionVisitor<TNode, TEdge, TUnit> visitor) => visitor.Visit(this);

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor) =>
            visitor.Visit(this);
    }
}