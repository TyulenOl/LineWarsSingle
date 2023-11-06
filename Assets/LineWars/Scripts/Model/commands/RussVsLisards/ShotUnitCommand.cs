using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class ShotUnitCommand<TNode, TEdge, TUnit> :
        ICommandWithCommandType
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private readonly IShotUnitAction<TNode, TEdge, TUnit> action;
        private readonly TUnit unit;
        private readonly TNode targetNode;
        private readonly TUnit takenUnit;

        public ShotUnitCommand(
            [NotNull] TUnit unit,
            [NotNull] TNode targetNode) :
            this(unit.TryGetUnitAction<IShotUnitAction<TNode, TEdge, TUnit>>(out var action)
                    ? action
                    : throw new ArgumentException(
                        $"{nameof(TUnit)} does not contain {nameof(IShotUnitAction<TNode, TEdge, TUnit>)}"),
                targetNode)
        {
        }

        public ShotUnitCommand(
            [NotNull] IShotUnitAction<TNode, TEdge, TUnit> action,
            [NotNull] TNode targetNode)
        {
            this.action = action;
            unit = action.MyUnit;
            this.targetNode = targetNode;
            this.takenUnit = action.TakenUnit;
        }

        public void Execute()
        {
            action.ShotUnitTo(targetNode);
        }

        public bool CanExecute()
        {
            return action.CanShotUnitTo(targetNode);
        }

        public string GetLog()
        {
            return $"Юнит {unit} бросил юнита {takenUnit} в ноду {targetNode}";
        }

        public CommandType CommandType => action.CommandType;
    }
}