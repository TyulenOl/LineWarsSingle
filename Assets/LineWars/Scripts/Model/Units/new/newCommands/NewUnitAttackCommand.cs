using UnityEngine;

namespace LineWars.Model
{
    public class NewUnitAttackCommand: ICommand
    {
        private readonly CombinedUnit attacker;
        private readonly IAlive defender;

        public NewUnitAttackCommand(CombinedUnit attacker, IAlive defender)
        {
            this.attacker = attacker;
            this.defender = defender;
        }


        public void Execute()
        {
            attacker.GetUnitAction<BaseAttackAction>().Attack(defender);
        }

        public bool CanExecute()
        {
            return attacker.TryGetUnitAction<BaseAttackAction>(out var action) 
                   && action.CanAttack(defender);
        }

        public string GetLog()
        {
            if (attacker is MonoBehaviour attackerUnit && defender is MonoBehaviour blockedUnit)
                return $"{attackerUnit.gameObject.name} атаковал {blockedUnit.gameObject.name}";
            return $"{attacker.GetType()} атаковал {defender.GetType()}";
        }
    }
}