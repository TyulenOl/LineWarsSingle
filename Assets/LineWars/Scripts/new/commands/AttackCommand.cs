using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace LineWars.Model
{
    public class AttackCommand: ICommand
    {
        private readonly AttackAction attackAction;

        protected readonly IUnit Attacker;
        protected readonly IAlive Defender;

        public AttackCommand([NotNull] IUnit attacker, [NotNull] IAlive defender)
        {
            Attacker = attacker ?? throw new ArgumentNullException(nameof(attacker));
            Defender = defender ?? throw new ArgumentNullException(nameof(defender));

            attackAction = attacker.TryGetExecutorAction<AttackAction>(out var action) 
                ? action 
                : throw new ArgumentException($"{nameof(ComponentUnit)} does not contain {nameof(ComponentUnit.BaseAttackAction)}");
        }

        public AttackCommand([NotNull] AttackAction attackAction, [NotNull] IAlive alive)
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
            return $"{Attacker} атаковал {Defender}";
        }
    }
}