namespace LineWars.Model
{
    public class MoveCommand: ICommand
    {
        private readonly IMovable movable;
        private readonly Node start;
        private readonly Node end;
        
        public MoveCommand(IMovable movable, Node start, Node end)
        {
            this.movable = movable;
            this.start = start;
            this.end = end;
        }
        
        public void Execute()
        {
            movable.MoveTo(end);
        }

        public bool CanExecute()
        {
            return movable.CanMoveTo(end);
        }

        public string GetLog()
        {
            if (movable is Unit unit)
                return $"Юнит {unit.gameObject.name} переместился из {start.gameObject.name} в {end.gameObject.name}";
            return $"Юнит {movable.GetType()} переместился из {start.gameObject.name} в {end.gameObject.name}";
        }
    }
}