using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class BlockCommand<TNode, TEdge, TUnit, TOwned, TPlayer> :
        ICommandWithCommandType
    
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TOwned, TPlayer>
        #endregion 
    {
        private readonly IBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer> blockAction;
        private readonly TUnit unit;

        public BlockCommand([NotNull] TUnit unit)
        {
            this.unit = unit;
            blockAction = unit.TryGetUnitAction<IBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer>>(out var action)
                    ? action
                    : throw new ArgumentException($"{nameof(TUnit)} does not contain {nameof(IBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer>)}");
        }

        public BlockCommand(IBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer> blockAction)
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