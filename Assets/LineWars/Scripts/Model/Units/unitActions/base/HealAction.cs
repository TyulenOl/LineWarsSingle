using System;
using System.Diagnostics.CodeAnalysis;


namespace LineWars.Model
{
    public class HealAction<TNode, TEdge, TUnit, TOwned, TPlayer> :
        UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>, 
        IHealAction<TNode, TEdge, TUnit, TOwned, TPlayer>
    
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TOwned, TPlayer>
        #endregion 
    {
        public bool IsMassHeal { get; private set; }
        public int HealingAmount { get; private set; }
        public bool HealLocked { get; private set; }
        
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
                return target.Owner == MyUnit.Owner;
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
            return new HealCommand<TNode, TEdge, TUnit, TOwned, TPlayer>(this, (TUnit) target);
        }

        public override void Accept(IUnitActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IIUnitActionVisitor<TResult, TNode, TEdge, TUnit, TOwned, TPlayer> visitor) => visitor.Visit(this);
    }
}