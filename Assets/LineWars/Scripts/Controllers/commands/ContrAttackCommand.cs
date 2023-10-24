using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class ContrAttackCommand<TNode, TEdge, TUnit, TOwned, TPlayer>:
        ICommand
    
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TOwned, TPlayer>
        #endregion 
    {
        private readonly IBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer> blockAction;
        private readonly TUnit attacker;
        private readonly TUnit blocker;

        public ContrAttackCommand([NotNull] TUnit attacker, [NotNull] TUnit blocker)
        {
            this.attacker = attacker ?? throw new ArgumentNullException(nameof(attacker));
            this.blocker = blocker ?? throw new ArgumentNullException(nameof(blocker));

            blockAction = attacker.TryGetUnitAction<IBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer>>(out var action)
                ? action
                : throw new ArgumentException($"{nameof(TUnit)} does not contain {nameof(IBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer>)}");
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
