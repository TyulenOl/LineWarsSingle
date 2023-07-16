using LineWars.Model;

namespace LineWars
{
    public class MoveCommand: ICommand
    {
        private readonly Unit unit;
        private readonly Node target;
        
        public MoveCommand(Unit unit, Node target)
        {
            this.unit = unit;
            this.target = target;
        }
        
        public void Execute()
        {
            unit.MoveTo(target);
        }

        public bool CanExecute()
        {
            return unit.IsCanMoveTo(target);
        }
    }
}