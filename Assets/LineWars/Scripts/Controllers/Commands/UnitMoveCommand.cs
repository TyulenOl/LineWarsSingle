using System;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class UnitMoveCommand: ICommand
    {
        private readonly ComponentUnit.MoveAction moveAction;
        private readonly ComponentUnit unit;
        private readonly Node start;
        private readonly Node end;
        
        public UnitMoveCommand([NotNull] ComponentUnit unit, [NotNull] Node end)
        {
            this.unit = unit ? unit : throw new ArgumentNullException(nameof(unit));
            this.end = end ? end : throw new ArgumentNullException(nameof(end));
            this.start = unit.Node;
            
            moveAction = unit.TryGetExecutorAction<ComponentUnit.MoveAction>(out var action) 
                ? action 
                : throw new ArgumentException($"{nameof(ComponentUnit)} does not contain {nameof(ComponentUnit.MoveAction)}");
        }

        public UnitMoveCommand([NotNull] ComponentUnit.MoveAction moveAction, [NotNull] Node end)
        {
            this.moveAction = moveAction ?? throw new ArgumentNullException(nameof(moveAction));
            this.end = end ? end : throw new ArgumentNullException(nameof(end));
            
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
            return $"Юнит {unit.gameObject.name} переместился из {start.gameObject.name} в {end.gameObject.name}";
        }
    }
}