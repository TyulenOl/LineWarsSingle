using System;
using JetBrains.Annotations;
using UnityEngine;

namespace LineWars.Model
{ 
    public class UnitAttackCommand : ICommand
    {
        private readonly ComponentUnit.BaseAttackAction attackAction;

        protected readonly ComponentUnit Attacker;
        protected readonly IAlive Defender;

        public UnitAttackCommand([NotNull] ComponentUnit attacker, [NotNull] IAlive defender)
        {
            Attacker = attacker ? attacker : throw new ArgumentNullException(nameof(attacker));
            Defender = defender ?? throw new ArgumentNullException(nameof(defender));

            attackAction = attacker.TryGetExecutorAction<ComponentUnit.BaseAttackAction>(out var action) 
                ? action 
                : throw new ArgumentException($"{nameof(ComponentUnit)} does not contain {nameof(ComponentUnit.BaseAttackAction)}");
        }

        public UnitAttackCommand([NotNull] ComponentUnit.BaseAttackAction attackAction, [NotNull] IAlive alive)
        {
            this.attackAction = attackAction ?? throw new ArgumentNullException(nameof(attackAction));

            Attacker = this.attackAction.MyUnit;
            Defender = alive ?? throw new ArgumentNullException(nameof(alive));
        }


        public void Execute()
        {
            attackAction.Attack(Defender);
        }

        public bool CanExecute()
        {
            return attackAction.CanAttack(Defender);
        }

        public virtual string GetLog()
        {
            if (Attacker is MonoBehaviour attackerUnit && Defender is MonoBehaviour blockedUnit)
                return $"{attackerUnit.gameObject.name} атаковал {blockedUnit.gameObject.name}";
            return $"{Attacker.GetType()} атаковал {Defender.GetType()}";
        }
    }
}