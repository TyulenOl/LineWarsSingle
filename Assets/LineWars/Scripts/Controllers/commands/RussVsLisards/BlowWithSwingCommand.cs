using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class BlowWithSwingCommand<TNode, TEdge, TUnit, TOwned, TPlayer> :
            ICommandWithCommandType
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion
    {
        private readonly IBlowWithSwingAction<TNode, TEdge, TUnit, TOwned, TPlayer> action;
        private readonly TUnit unit;

        public BlowWithSwingCommand(
            [NotNull] TUnit unit) : this(
            unit.TryGetUnitAction<IBlowWithSwingAction<TNode, TEdge, TUnit, TOwned, TPlayer>>(out var action)
                ? action
                : throw new ArgumentException($"{nameof(TUnit)} does not contain {nameof(IBlowWithSwingAction<TNode, TEdge, TUnit, TOwned, TPlayer>)}")){}

        public BlowWithSwingCommand(
            [NotNull] IBlowWithSwingAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            unit = action.MyUnit;
        }

        public void Execute()
        {
            action.ExecuteBlowWithSwing();
        }

        public bool CanExecute()
        {
            return action.CanBlowWithSwing();
        }

        public string GetLog()
        {
            return $"Юнит {unit} нанес круговую атаку";
        }

        public CommandType CommandType => action.CommandType;
    }
}