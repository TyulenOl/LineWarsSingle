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

        public bool IsAvailable1([NotNull] TUnit unit)
        {
            if (unit == null) throw new ArgumentNullException(nameof(unit));

            var line = MyUnit.Node.GetLine(unit.Node);
            return ActionPointsCondition()
                   && line != null;
        }

        public bool IsAvailable2(TNode target2)
        {
            return true;
        }

        public void Execute(TUnit takenUnit, TNode node)
        {
            if (takenUnit.Node.LeftUnit == takenUnit)
                takenUnit.Node.LeftUnit = null;
            if (takenUnit.Node.RightUnit == takenUnit)
                takenUnit.Node.RightUnit = null;
            
            if (node.AllIsFree)
            {
                takenUnit.Node = node;
            }
            else
            {
                var enemies = node.Units.ToArray();
                var myDamage = (takenUnit.CurrentHp + takenUnit.CurrentArmor) / enemies.Length;
                var enemyDamage = enemies.Select(x => x.CurrentHp + x.CurrentArmor).Sum();
                foreach (var enemy in enemies)
                    enemy.DealDamageThroughArmor(myDamage);
                takenUnit.DealDamageThroughArmor(enemyDamage);
            }
            
            
            CompleteAndAutoModify();
        }
        public override void Accept(IUnitActionVisitor<TNode, TEdge, TUnit> visitor) => visitor.Visit(this);

        public override TResult Accept<TResult>(IIUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor) =>
            visitor.Visit(this);
    }
}