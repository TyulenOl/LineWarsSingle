using System;
using System.Diagnostics.CodeAnalysis;


namespace LineWars.Model
{
    public class HealAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>, 
        IHealAction<TNode, TEdge, TUnit>
    
        #region Сonstraints
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit> 
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
        #endregion 
    {
        public bool IsMassHeal { get;  set; }
        public int HealingAmount { get;  set; }
        public bool HealLocked { get;  set; }
        
        public HealAction(TUnit executor, bool isMassHeal, int healingAmount) : base(executor)
        {
            IsMassHeal = isMassHeal;
            HealingAmount = healingAmount;
        }
        
        public bool CanHeal([NotNull] TUnit target, bool ignoreActionPointsCondition = false)
        {
            return !HealLocked
                   && OwnerCondition()
                   && SpaceCondition()
                   && (ignoreActionPointsCondition || ActionPointsCondition())
                   && target != MyUnit
                   && target.CurrentHp != target.MaxHp;

            bool SpaceCondition()
            {
                var line = MyUnit.Node.GetLine(target.Node);
                return line != null || MyUnit.IsNeighbour(target);
            }

            bool OwnerCondition()
            {
                return target.OwnerId == MyUnit.OwnerId;
            }
        }

        public void Heal([NotNull] TUnit target)
        {
            target.CurrentHp += HealingAmount;
            if (IsMassHeal && MyUnit.TryGetNeighbour(out var neighbour))
                neighbour.CurrentHp += HealingAmount;


            CompleteAndAutoModify();
        }

        public override CommandType CommandType => CommandType.Heal;

        public Type TargetType => typeof(TUnit);
        public bool IsMyTarget(ITarget target) => target is TUnit;

        public ICommandWithCommandType GenerateCommand(ITarget target)
        {
            return new HealCommand<TNode, TEdge, TUnit>(this, (TUnit) target);
        }

        public override void Accept(IUnitActionVisitor<TNode, TEdge, TUnit> visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IIUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor) => visitor.Visit(this);
    }
}