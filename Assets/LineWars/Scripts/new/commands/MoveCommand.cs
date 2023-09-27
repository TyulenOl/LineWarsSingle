using System;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public class MoveCommand: ICommand
    {
        private readonly MoveAction moveAction;
        private readonly IUnit unit;
        private readonly INode start;
        private readonly INode end;
        
        public MoveCommand([NotNull] IUnit unit, [NotNull] INode end)
        {
            this.unit = unit ?? throw new ArgumentNullException(nameof(unit));
            this.end = end ?? throw new ArgumentNullException(nameof(end));
            this.start = unit.Node;
            
            moveAction = unit.TryGetExecutorAction<MoveAction>(out var action) 
                ? action 
                : throw new ArgumentException($"{unit} does not contain {nameof(MoveAction)}");
        }

        public MoveCommand([NotNull] MoveAction moveAction, [NotNull] INode end)
        {
            this.moveAction = moveAction ?? throw new ArgumentNullException(nameof(moveAction));
            this.end = end ?? throw new ArgumentNullException(nameof(end));
            
            unit = this.moveAction.MyUnit;
            start = unit.Node;
        }

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