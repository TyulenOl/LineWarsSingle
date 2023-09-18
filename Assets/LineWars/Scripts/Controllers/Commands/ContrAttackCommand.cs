using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public class ContrAttackCommand: ICommand
    {
        private readonly ComponentUnit attacker;
        private readonly ComponentUnit blocker;
        
        public ContrAttackCommand([NotNull] ComponentUnit attacker, [NotNull] ComponentUnit blocker)
        {
            this.attacker = attacker;
            this.blocker = blocker;
        }
        
        public void Execute()
        {
            attacker.GetExecutorAction<ComponentUnit.ContAttackAction>()
                .ContrAttack(blocker);
        }

        public bool CanExecute()
        {
            return attacker.TryGetExecutorAction<ComponentUnit.ContAttackAction>(out var action)
                   && action.CanContrAttack(blocker);
        }

        public string GetLog()
        {
            return $"Юнит {attacker.gameObject.name} контратаковал юнита {blocker.gameObject.name}";
        }
    }
}