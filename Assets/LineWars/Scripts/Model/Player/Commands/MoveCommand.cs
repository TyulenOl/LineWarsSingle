namespace LineWars.Model
{
    public class MoveCommand: ICommand
    {
        private readonly IMovable unit;
        private readonly Node start;
        private readonly Node end;
        
        public MoveCommand(IMovable unit, Node start, Node end)
        {
            this.unit = unit;
            this.start = start;
            this.end = end;
        }
        
        public void Execute()
        {
            unit.MoveTo(end);
        }

        public bool CanExecute()
        {
            return unit.CanMoveTo(end);
        }
    }
}