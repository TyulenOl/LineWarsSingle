using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class TakeUnitCommand<TNode, TEdge, TUnit> :
        IActionCommand
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private readonly IShotUnitAction<TNode, TEdge, TUnit> action;
        private readonly TUnit unit;
        private readonly TUnit targetUnit;

        public TakeUnitCommand(
            [NotNull] TUnit unit,
            [NotNull] TUnit targetUnit) :
            this(unit.TryGetUnitAction<IShotUnitAction<TNode, TEdge, TUnit>>(out var action)
                    ? action
                    : throw new ArgumentException(
                        $"{nameof(TUnit)} does not contain {nameof(IShotUnitAction<TNode, TEdge, TUnit>)}"),
                targetUnit)
        {
        }

        public TakeUnitCommand(
            [NotNull] IShotUnitAction<TNode, TEdge, TUnit> action,
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
        public ActionType ActionType => action.ActionType;
    }
}