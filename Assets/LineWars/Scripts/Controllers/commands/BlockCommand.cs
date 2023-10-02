using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class BlockCommand<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> :
        ICommand
    
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TPlayer : class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TNation : class, INation<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
    {
        private readonly IBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> blockAction;
        private readonly TUnit unit;

        public BlockCommand([NotNull] TUnit unit)
        {
            this.unit = unit;
            blockAction = unit.TryGetUnitAction<IBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>>(out var action)
                    ? action
                    : throw new ArgumentException($"{nameof(TUnit)} does not contain {nameof(IBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>)}");
        }

        public BlockCommand(IBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> blockAction)
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
    }
}