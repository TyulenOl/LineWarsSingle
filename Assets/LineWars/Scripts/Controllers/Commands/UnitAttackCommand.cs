using System.Diagnostics.CodeAnalysis;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Model
{
    public class UnitAttackCommand : ICommand
    {
        protected readonly ComponentUnit attacker;
        protected readonly IAlive defender;

        public UnitAttackCommand(ComponentUnit attacker, IAlive defender)
        {
            this.attacker = attacker;
            this.defender = defender;
        }


        public void Execute()
        {
            attacker.GetExecutorAction<ComponentUnit.BaseAttackAction>()
                .Attack(defender);
        }

        public bool CanExecute()
        {
            return attacker.TryGetExecutorAction<ComponentUnit.BaseAttackAction>(out var action) 
                   && action.CanAttack(defender);
        }

        public virtual string GetLog()
        {
            if (attacker is MonoBehaviour attackerUnit && defender is MonoBehaviour blockedUnit)
                return $"{attackerUnit.gameObject.name} атаковал {blockedUnit.gameObject.name}";
            return $"{attacker.GetType()} атаковал {defender.GetType()}";
        }
    }
}