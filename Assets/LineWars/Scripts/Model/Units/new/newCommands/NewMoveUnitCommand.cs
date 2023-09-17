namespace LineWars.Model
{
    public class NewMoveUnitCommand: ICommand
    {
        private readonly CombinedUnit unit;
        private readonly Node start;
        private readonly Node end;
        
        public NewMoveUnitCommand(CombinedUnit unit, Node start, Node end)
        {
            this.unit = unit;
            this.start = start;
            this.end = end;
        }
        
        public void Execute()
        {
            unit.GetUnitAction<MoveAction>().MoveTo(end);
        }

        public bool CanExecute()
        {
            return unit.TryGetUnitAction<MoveAction>(out var action)
                   && action.CanMoveTo(end);
        }

        public string GetLog()
        {
            return $"Юнит {unit.gameObject.name} переместился из {start.gameObject.name} в {end.gameObject.name}";
        }
    }
}