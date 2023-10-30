using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class TakeUnitCommand<TNode, TEdge, TUnit, TOwned, TPlayer> :
            ICommandWithCommandType

        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion

    {
        private readonly IShotUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer> action;
        private readonly TUnit unit;
        private readonly TUnit targetUnit;

        public TakeUnitCommand(
            [NotNull] TUnit unit,
            [NotNull] TUnit targetUnit) :
            this(unit.TryGetUnitAction<IShotUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>>(out var action)
                    ? action
                    : throw new ArgumentException($"{nameof(TUnit)} does not contain {nameof(IShotUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>)}"),
                targetUnit)
        {
        }

        public TakeUnitCommand(
            [NotNull] IShotUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer> action,
            [NotNull] TUnit targetUnit)
        {
            this.action = action;
            unit = action.MyUnit;
            this.targetUnit = targetUnit;
        }

        public void Execute()
        {
            action.TakeUnit(targetUnit);
        }

        public bool CanExecute()
        {
            return action.CanTakeUnit(targetUnit);
        }

        public string GetLog()
        {
            return $"Юнит {unit} взял юнита {targetUnit}";
        }

        public CommandType CommandType => action.CommandType;
    }
}