using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class RLBlockCommand<TNode, TEdge, TUnit> :
        ICommandWithCommandType
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private readonly IRLBlockAction<TNode, TEdge, TUnit> action;
        private readonly TUnit unit;

        public RLBlockCommand([NotNull] TUnit unit) : this(
            unit.TryGetUnitAction<IRLBlockAction<TNode, TEdge, TUnit>>(out var action)
                ? action
                : throw new ArgumentException(
                    $"{nameof(TUnit)} does not contain {nameof(IRLBlockAction<TNode, TEdge, TUnit>)}")
        )
        {
        }

        public RLBlockCommand(IRLBlockAction<TNode, TEdge, TUnit> action)
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