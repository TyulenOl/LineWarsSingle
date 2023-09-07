using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public class BlockAttackCommand: ICommand
    {
        private readonly Unit attacker;
        private readonly Unit blocker;
        
        public BlockAttackCommand([NotNull] Unit attacker, [NotNull] Unit blocker)
        {
            this.attacker = attacker;
            this.blocker = blocker;
        }

        public void Execute()
        {
            attacker.AttackUnitButIgnoreBlock(blocker);
        }

        public bool CanExecute()
        {
            return attacker.CanAttack(blocker);
        }

        public string GetLog()
        {
            return $"{blocker.gameObject.name} перехватил атаку от {attacker.gameObject.name}";
        }
    }
}