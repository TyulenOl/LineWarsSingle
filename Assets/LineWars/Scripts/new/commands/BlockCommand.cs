using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class BlockCommand : ICommand
    {
        private readonly BlockAction blockAction;
        private readonly IUnit unit;
        public BlockCommand([NotNull] IUnit unit)
        {
            this.unit = unit;
            blockAction = unit.TryGetExecutorAction<BlockAction>(out var action)
                ? action
                : throw new ArgumentException($"{nameof(IUnit)} does not contain {nameof(BlockAction)}");
        }

        public BlockCommand(BlockAction blockAction)
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
