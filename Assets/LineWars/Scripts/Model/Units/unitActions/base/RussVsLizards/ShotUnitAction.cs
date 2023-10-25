using System;
using System.Linq;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class ShotUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer> :
        UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>,
        IShotUnitActon<TNode, TEdge, TUnit, TOwned, TPlayer>

        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion
    {
        public ShotUnitAction(TUnit unit, MonoShotUnitAction data)
            : base(unit, data)
        {
        }

        public ShotUnitAction(TUnit unit, ShotUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer> data)
            : base(unit, data)
        {
        }

        public override CommandType CommandType => CommandType.ShotUnit;

        public override void Accept(IUnitActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> visitor) =>
            visitor.Visit(this);

        
        public TUnit TakenUnit { get; private set; }
        public bool CanTakeUnit([NotNull] TUnit unit)
        {
            if (unit == null) throw new ArgumentNullException(nameof(unit));
            
            var line = MyUnit.Node.GetLine(unit.Node);
            return ActionPointsCondition()
                   && line != null;
        }

        public void TakeUnit([NotNull] TUnit unit)
        {
            if (unit == null) throw new ArgumentNullException(nameof(unit));

            TakenUnit = unit;
        }

        public bool CanShotUnitTo(TNode node)
        {
            return TakenUnit != null;
        }

        public void ShotUnit(TNode node)
        {
            if (node.AllIsFree)
            {
                TakenUnit.Node = node;
            }
            else
            {
                var enemies = node.Units
                    .ToArray();
                var myDamage = (TakenUnit.CurrentHp + TakenUnit.CurrentArmor) / enemies.Length;
                var enemyDamage = enemies.Select(x => x.CurrentHp + x.CurrentArmor).Sum();
                foreach (var enemy in enemies)
                    enemy.DealDamageThroughArmor(myDamage);
                TakenUnit.DealDamageThroughArmor(enemyDamage);
            }
            TakenUnit = null;
            CompleteAndAutoModify();
        }
    }
}