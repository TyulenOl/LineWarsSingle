using System;

namespace LineWars.Model
{
    public class RLBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer> :
            UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>,
            IRLBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer>, ISimpleAction

        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion

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

        public bool CanBlock() => ActionPointsCondition();

        public void EnableBlock()
        {
            MyUnit.CurrentArmor = MyUnit.CurrentActionPoints - ModifyActionPoints();
            CompleteAndAutoModify();
        }

        public override CommandType CommandType => CommandType.Block;

        public override void Accept(IUnitActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> visitor) =>
            visitor.Visit(this);

        public ICommandWithCommandType GenerateCommand() => new RLBlockCommand<TNode, TEdge, TUnit, TOwned, TPlayer>(this);

        public RLBlockAction(TUnit executor) : base(executor)
        {
        }
    }
}