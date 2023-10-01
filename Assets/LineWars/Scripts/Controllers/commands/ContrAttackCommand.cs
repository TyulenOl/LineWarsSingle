using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class ContrAttackCommand<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>:
        ICommand
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TPlayer : class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TNation : class, INation<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
    {
        private readonly BlockAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> blockAction;
        private readonly TUnit attacker;
        private readonly TUnit blocker;

        public ContrAttackCommand([NotNull] TUnit attacker, [NotNull] TUnit blocker)
        {
            this.attacker = attacker ?? throw new ArgumentNullException(nameof(attacker));
            this.blocker = blocker ?? throw new ArgumentNullException(nameof(blocker));

            blockAction = attacker.TryGetUnitAction<BlockAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>>(out var action)
                ? action
                : throw new ArgumentException($"{nameof(TUnit)} does not contain {nameof(BlockAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>)}");
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
