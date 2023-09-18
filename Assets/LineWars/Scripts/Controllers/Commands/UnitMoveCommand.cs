namespace LineWars.Model
{
    public class UnitMoveCommand: ICommand
    {
        private readonly ComponentUnit unit;
        private readonly Node start;
        private readonly Node end;
        
        public UnitMoveCommand(ComponentUnit unit, Node start, Node end)
        {
            this.unit = unit;
            this.start = start;
            this.end = end;
        }
        
        public void Execute()
        {
            unit.GetExecutorAction<ComponentUnit.MoveAction>().MoveTo(end);
        }

        public bool CanExecute()
        {
            return unit.TryGetExecutorAction<ComponentUnit.MoveAction>(out var action)
                   && action.CanMoveTo(end);
        }

        public string GetLog()
        {
            return $"Юнит {unit.gameObject.name} переместился из {start.gameObject.name} в {end.gameObject.name}";
        }
    }
}