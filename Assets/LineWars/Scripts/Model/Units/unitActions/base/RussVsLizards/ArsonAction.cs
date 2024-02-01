namespace LineWars.Model
{
    public class ArsonAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IArsonAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private int fireEffectRounds;
        public override CommandType CommandType => CommandType.Arson;
        public int FireEffectRounds => fireEffectRounds;
        public ArsonAction(TUnit executor, int fireEffectRounds) : base(executor)
        {
            this.fireEffectRounds = fireEffectRounds;
        }

        public bool IsAvailable(TNode target)
        { 
            var line = Executor.Node.GetLine(target);
            return ActionPointsCondition()
                    && line != null
                    && Executor.CanMoveOnLineWithType(line.LineType)
                    && target.OwnerId != Executor.OwnerId
                    && !target.AllIsFree;
        }

        public void Execute(TNode target)
        {
            if(!target.LeftIsFree && target.LeftUnit.Size == UnitSize.Little)
            {
                FireUnit(target.LeftUnit);
            }
            if(!target.RightIsFree)
            {
                FireUnit(target.RightUnit);
            }
            CompleteAndAutoModify();
        }

        private void FireUnit(TUnit unit)
        {
            var effect = new FireEffect<TNode, TEdge, TUnit>
                (unit, fireEffectRounds, Executor.CurrentPower);
            unit.AddEffect(effect);
        }

        public IActionCommand GenerateCommand(TNode target)
        {
            return new TargetedUniversalCommand<
                TUnit,
                ArsonAction<TNode, TEdge, TUnit>,
                TNode>(Executor, target);
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
