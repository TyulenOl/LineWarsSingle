using System;
using JetBrains.Annotations;
using UnityEngine;

namespace LineWars.Model
{ 
    public class UnitAttackCommand : ICommand
    {
        private readonly ModelComponentUnit.BaseAttackAction attackAction;

        protected readonly ModelComponentUnit Attacker;
        protected readonly IAlive Defender;

        public UnitAttackCommand([NotNull] ModelComponentUnit attacker, [NotNull] IAlive defender)
        {
            Attacker = attacker ?? throw new ArgumentNullException(nameof(attacker));
            Defender = defender ?? throw new ArgumentNullException(nameof(defender));

            attackAction = attacker.TryGetExecutorAction<ModelComponentUnit.BaseAttackAction>(out var action) 
                ? action 
                : throw new ArgumentException($"{nameof(ModelComponentUnit)} does not contain {nameof(ModelComponentUnit.BaseAttackAction)}");
        }

        public UnitAttackCommand([NotNull] ModelComponentUnit.BaseAttackAction attackAction, [NotNull] IAlive alive)
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