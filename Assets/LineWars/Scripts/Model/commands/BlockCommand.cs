using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class BlockCommand<TNode, TEdge, TUnit> :
        ICommandWithCommandType
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private readonly IBlockAction<TNode, TEdge, TUnit> blockAction;
        private readonly TUnit unit;

        public BlockCommand([NotNull] TUnit unit)
        {
            this.unit = unit;
            blockAction = unit.TryGetUnitAction<IBlockAction<TNode, TEdge, TUnit>>(out var action)
                ? action
                : throw new ArgumentException(
                    $"{nameof(TUnit)} does not contain {nameof(IBlockAction<TNode, TEdge, TUnit>)}");
        }

        public BlockCommand(IBlockAction<TNode, TEdge, TUnit> blockAction)
        {
            this.blockAction = blockAction;
            unit = blockAction.MyUnit;
        }

        public void Execute()
        {
            blockAction.EnableBlock();
        }

        public bool CanExecute()
        {
            return blockAction.CanBlock();
        }

        public string GetLog()
        {
            return $"Юнит {unit} встал в защиту";
        }

        public CommandType CommandType => blockAction.CommandType;
    }
}