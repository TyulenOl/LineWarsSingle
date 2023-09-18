using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public class EnableBlockCommand: ICommand
    {
        private readonly ComponentUnit unit;
        public EnableBlockCommand([NotNull] ComponentUnit unit)
        {
            this.unit = unit;
        }
        public void Execute()
        {
            unit.GetExecutorAction<ComponentUnit.ContAttackAction>().EnableBlock();
        }

        public bool CanExecute()
        {
            return unit.TryGetExecutorAction<ComponentUnit.ContAttackAction>(out var action)
                   && action.CanBlock();
        }

        public string GetLog()
        {
            return $"Юнит {unit.gameObject.name} встал в защиту";
        }
    }
}