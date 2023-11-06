using System;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public class AttackCommand<TNode, TEdge, TUnit> :
        ICommandWithCommandType
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private readonly IAttackAction<TNode, TEdge, TUnit> attackAction;

        protected readonly TUnit Attacker;
        protected readonly IAlive Defender;

        public AttackCommand([NotNull] TUnit attacker, [NotNull] IAlive defender)
        {
            Attacker = attacker ?? throw new ArgumentNullException(nameof(attacker));
            Defender = defender ?? throw new ArgumentNullException(nameof(defender));

            attackAction = attacker.TryGetUnitAction<IAttackAction<TNode, TEdge, TUnit>>(out var action)
                ? action
                : throw new ArgumentException(
                    $"{Attacker} does not contain {nameof(IAttackAction<TNode, TEdge, TUnit>)}");
        }

        public AttackCommand(
            [NotNull] IAttackAction<TNode, TEdge, TUnit> attackAction,
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

        public CommandType CommandType => attackAction.CommandType;
    }
}