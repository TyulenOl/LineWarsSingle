using LineWars.Model;

namespace LineWars.Model
{
    public class AttackCommand : ICommand
    {
        private readonly IAttackerVisitor attackerVisitor;
        private readonly IAlive unit;

        public AttackCommand(IAttackerVisitor attackerVisitor, IAlive unit)
        {
            this.attackerVisitor = attackerVisitor;
            this.unit = unit;
        }

        public void Execute()
        {
            attackerVisitor.Attack(unit);
        }

        public bool CanExecute()
        {
            return attackerVisitor.CanAttack(unit);
        }

    }
}