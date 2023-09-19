using System;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public class EnableBlockCommand: ICommand
    {
        private readonly ComponentUnit.BlockAction blockAction;
        private readonly ComponentUnit unit;
        public EnableBlockCommand([NotNull] ComponentUnit unit)
        {
            this.unit = unit;
            blockAction = unit.TryGetExecutorAction<ComponentUnit.BlockAction>(out var action) 
                ? action 
                : throw new ArgumentException($"{nameof(ComponentUnit)} does not contain {nameof(ComponentUnit.BlockAction)}");
        }

        public EnableBlockCommand(ComponentUnit.BlockAction blockAction)
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
            return $"Юнит {unit.gameObject.name} встал в защиту";
        }
    }
}