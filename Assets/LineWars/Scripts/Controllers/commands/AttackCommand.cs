using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace LineWars.Model
{
    public class AttackCommand<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>:
        ICommand
    
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TPlayer : class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TNation : class, INation<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
    {
        private readonly IAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> attackAction;

        protected readonly TUnit Attacker;
        protected readonly IAlive Defender;

        public AttackCommand([NotNull] TUnit attacker, [NotNull] IAlive defender)
        {
            Attacker = attacker ?? throw new ArgumentNullException(nameof(attacker));
            Defender = defender ?? throw new ArgumentNullException(nameof(defender));

            attackAction = attacker.TryGetUnitAction<IAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>>(out var action) 
                ? action 
                : throw new ArgumentException($"{Attacker} does not contain {nameof(IAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>)}");
        }

        public AttackCommand(
            [NotNull] IAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> attackAction,
            [NotNull] IAlive alive)
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