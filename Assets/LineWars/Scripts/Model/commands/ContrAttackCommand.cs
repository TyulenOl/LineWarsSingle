using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class ContrAttackCommand<TNode, TEdge, TUnit> :
        ICommand
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private readonly IBlockAction<TNode, TEdge, TUnit> blockAction;
        private readonly TUnit attacker;
        private readonly TUnit blocker;

        public ContrAttackCommand([NotNull] TUnit attacker, [NotNull] TUnit blocker)
        {
            this.attacker = attacker ?? throw new ArgumentNullException(nameof(attacker));
            this.blocker = blocker ?? throw new ArgumentNullException(nameof(blocker));

            blockAction = attacker.TryGetUnitAction<IBlockAction<TNode, TEdge, TUnit>>(out var action)
                ? action
                : throw new ArgumentException(
                    $"{nameof(TUnit)} does not contain {nameof(IBlockAction<TNode, TEdge, TUnit>)}");
        }

        public void Execute()
        {
            blockAction.ContrAttack(blocker);
        }

        public bool CanExecute()
        {
            return blockAction.CanContrAttack(blocker);
        }

        public string GetLog()
        {
            return $"Юнит {attacker} контратаковал юнита {blocker}";
        }
    }
}