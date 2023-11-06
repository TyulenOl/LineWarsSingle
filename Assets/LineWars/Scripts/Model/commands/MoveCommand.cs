using System;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public class MoveCommand<TNode, TEdge, TUnit> :
        ICommandWithCommandType
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private readonly IMoveAction<TNode, TEdge, TUnit> moveAction;
        private readonly TUnit unit;
        private readonly TNode start;
        private readonly TNode end;

        public MoveCommand(
            [NotNull] TUnit unit,
            [NotNull] TNode end)
        {
            this.unit = unit ?? throw new ArgumentNullException(nameof(unit));
            this.end = end ?? throw new ArgumentNullException(nameof(end));
            this.start = unit.Node;

            moveAction = unit.TryGetUnitAction<IMoveAction<TNode, TEdge, TUnit>>(out var action)
                ? action
                : throw new ArgumentException($"{unit} does not contain {nameof(IMoveAction<TNode, TEdge, TUnit>)}");
        }

        public MoveCommand(
            [NotNull] IMoveAction<TNode, TEdge, TUnit> moveAction,
            [NotNull] TNode end)
        {
            this.moveAction = moveAction ?? throw new ArgumentNullException(nameof(moveAction));
            this.end = end ?? throw new ArgumentNullException(nameof(end));

            unit = this.moveAction.MyUnit;
            start = unit.Node;
        }

        public CommandType CommandType => moveAction.CommandType;

        public void Execute()
        {
            moveAction.MoveTo(end);
        }

        public bool CanExecute()
        {
            return moveAction.CanMoveTo(end);
        }

        public string GetLog()
        {
            return $"Юнит {unit} переместился из {start} в {end}";
        }
    }
}