namespace LineWars.Model
{
    public class BlockAttackCommand: ICommand
    {
        private Unit attacker;
        private Unit blocker;
        
        public BlockAttackCommand(Unit attacker, Unit blocker)
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
    }
}