using LineWars.Model;

namespace LineWars.Model
{
    public class AttackCommand : ICommand
    {
        private readonly IAttackerVisitor attackerVisitor;
        private readonly IAlive alive;

        public AttackCommand(IAttackerVisitor attackerVisitor, IAlive alive)
        {
            this.attackerVisitor = attackerVisitor;
            this.alive = alive;
        }

        public void Execute()
        {
            attackerVisitor.Attack(alive);
        }

        public bool CanExecute()
        {
            return attackerVisitor.CanAttack(alive);
        }

    }
}