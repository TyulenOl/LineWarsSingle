using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class RamCommand<TNode, TEdge, TUnit, TOwned, TPlayer> : 
            ICommandWithCommandType
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion

    {
        private readonly IRamAction<TNode, TEdge, TUnit, TOwned, TPlayer> action;
        private readonly TUnit unit;
        private readonly TNode targetNode;

        public RamCommand(
            [NotNull] TUnit unit,
            [NotNull] TNode targetNode) : this(
            unit.TryGetUnitAction<IRamAction<TNode, TEdge, TUnit, TOwned, TPlayer>>(out var action)
                ? action
                : throw new ArgumentException($"{nameof(TUnit)} does not contain {nameof(IRamAction<TNode, TEdge, TUnit, TOwned, TPlayer>)}"),
            targetNode){}

        public RamCommand(
            [NotNull] IRamAction<TNode, TEdge, TUnit, TOwned, TPlayer> ramAction,
            [NotNull] TNode targetNode)
        {
            this.action = ramAction ?? throw new ArgumentNullException(nameof(ramAction));
            unit = ramAction.MyUnit;
            this.targetNode = targetNode ?? throw new ArgumentNullException(nameof(targetNode));

        }

        public void Execute()
        {
            action.Ram(targetNode);
        }

        public bool CanExecute()
        {
            return action.CanRam(targetNode);
        }

        public string GetLog()
        {
            return $"Юнит {unit} протаранил ноду {targetNode}";
        }
        
        public CommandType CommandType => action.CommandType;
    }
}