using System;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public class AttackCommand<TNode, TEdge, TUnit, TOwned, TPlayer>:
        ICommand
    
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer>
        #endregion 
    {
        private readonly IAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> attackAction;

        protected readonly TUnit Attacker;
        protected readonly IAlive Defender;

        public AttackCommand([NotNull] TUnit attacker, [NotNull] IAlive defender)
        {
            Attacker = attacker ?? throw new ArgumentNullException(nameof(attacker));
            Defender = defender ?? throw new ArgumentNullException(nameof(defender));

            attackAction = attacker.TryGetUnitAction<IAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer>>(out var action) 
                ? action 
                : throw new ArgumentException($"{Attacker} does not contain {nameof(IAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer>)}");
        }

        public AttackCommand(
            [NotNull] IAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> attackAction,
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