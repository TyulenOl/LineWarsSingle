using System;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public class EnableBlockCommand: ICommand
    {
        private readonly ModelComponentUnit.BlockAction blockAction;
        private readonly ModelComponentUnit unit;
        public EnableBlockCommand([NotNull] ModelComponentUnit unit)
        {
            this.unit = unit;
            blockAction = unit.TryGetExecutorAction<ModelComponentUnit.BlockAction>(out var action) 
                ? action 
                : throw new ArgumentException($"{nameof(ModelComponentUnit)} does not contain {nameof(ModelComponentUnit.BlockAction)}");
        }

        public EnableBlockCommand(ModelComponentUnit.BlockAction blockAction)
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