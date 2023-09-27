using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class UnitMoveCommand: ICommand
    {
        private readonly ModelComponentUnit.MoveAction moveAction;
        private readonly ModelComponentUnit unit;
        private readonly ModelNode start;
        private readonly ModelNode end;
        
        public UnitMoveCommand([NotNull] ModelComponentUnit unit, [NotNull] ModelNode end)
        {
            this.unit = unit ?? throw new ArgumentNullException(nameof(unit));
            this.end = end ?? throw new ArgumentNullException(nameof(end));
            this.start = unit.Node;
            
            moveAction = unit.TryGetExecutorAction<ModelComponentUnit.MoveAction>(out var action) 
                ? action 
                : throw new ArgumentException($"{nameof(ModelComponentUnit)} does not contain {nameof(ModelComponentUnit.MoveAction)}");
        }

        public UnitMoveCommand([NotNull] ModelComponentUnit.MoveAction moveAction, [NotNull] ModelNode end)
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