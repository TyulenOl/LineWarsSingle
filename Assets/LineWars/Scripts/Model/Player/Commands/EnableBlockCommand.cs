using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public class EnableBlockCommand: ICommand
    {
        private Unit unit;
        public EnableBlockCommand([NotNull] Unit unit)
        {
            this.unit = unit;
        }
        public void Execute()
        {
            unit.EnableBlock();
        }

        public bool CanExecute()
        {
            return unit.CanBlock();
        }
    }
}