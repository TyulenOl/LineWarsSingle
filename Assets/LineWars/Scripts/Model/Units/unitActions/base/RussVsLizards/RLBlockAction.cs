using System;

namespace LineWars.Model
{
    public class RLBlockAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IRLBlockAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        private bool isBlocked;

        public bool IsBlocked
        {
            get => isBlocked;
            private set
            {
                var previous = isBlocked;
                if (isBlocked == previous)
                    return;

                isBlocked = value;
                CanBlockChanged?.Invoke(previous, isBlocked);
            }
        }

        public event Action<bool, bool> CanBlockChanged;

        public override CommandType CommandType => CommandType.Block;
        public override ActionType ActionType => ActionType.Simple;
        
        public RLBlockAction(TUnit executor) : base(executor)
        {
        }

        public bool CanBlock() => ActionPointsCondition();

        public void EnableBlock()
        {
            MyUnit.CurrentArmor = MyUnit.CurrentActionPoints - ModifyActionPoints();
            CompleteAndAutoModify();
        }

        public override void OnReplenish()
        {
            base.OnReplenish();
            if (isBlocked)
            {
                isBlocked = false;
                MyUnit.CurrentArmor = 0;
            }
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