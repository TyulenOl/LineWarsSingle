using System.Diagnostics.CodeAnalysis;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Model
{
    public class AttackCommand : ICommand
    {
        private readonly IAttackerVisitor attacker;
        private readonly IAlive blocked;

        public AttackCommand([NotNull] IAttackerVisitor attacker, [NotNull] IAlive blocked)
        {
            this.attacker = attacker;
            this.blocked = blocked;
        }

        public void Execute()
        {
            attacker.Attack(blocked);
        }

        public bool CanExecute()
        {
            return attacker.CanAttack(blocked);
        }

        public string GetLog()
        {
            if (attacker is MonoBehaviour attackerUnit && blocked is MonoBehaviour blockedUnit)
                return $"{attackerUnit.gameObject.name} атаковал {blockedUnit.gameObject.name}";
            return $"{attacker.GetType()} атаковал {blocked.GetType()}";
        }
    }
}