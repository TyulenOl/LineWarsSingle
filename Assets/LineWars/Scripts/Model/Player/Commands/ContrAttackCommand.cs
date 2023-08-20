using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public class ContrAttackCommand: ICommand
    {
        private readonly Unit attacker;
        private readonly Unit blocker;
        
        public ContrAttackCommand([NotNull] Unit attacker, [NotNull] Unit blocker)
        {
            this.attacker = attacker;
            this.blocker = blocker;
        }
        
        public void Execute()
        {
            attacker.ContrAttack(blocker);
        }

        public bool CanExecute()
        {
            return attacker.CanContrAttack(blocker);
        }
    }
}