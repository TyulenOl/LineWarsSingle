using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class RLBlockCommand<TNode, TEdge, TUnit, TOwned, TPlayer> :
            ICommandWithCommandType
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion
    {
        private readonly IRLBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer> action;
        private readonly TUnit unit;

        public RLBlockCommand([NotNull] TUnit unit) : this(
            unit.TryGetUnitAction<IRLBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer>>(out var action)
                ? action
                : throw new ArgumentException($"{nameof(TUnit)} does not contain {nameof(IRLBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer>)}")
            ){}

        public RLBlockCommand(IRLBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            this.action = action;
            unit = action.MyUnit;
        }

        public void Execute()
        {
            action.EnableBlock();
        }

        public bool CanExecute()
        {
            return action.CanBlock();
        }

        public string GetLog()
        {
            return $"Юнит {unit} встал в защиту";
        }
        
        public CommandType CommandType => action.CommandType;
    }
}