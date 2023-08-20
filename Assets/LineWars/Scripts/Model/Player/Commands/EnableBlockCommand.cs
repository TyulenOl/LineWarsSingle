namespace LineWars.Model
{
    public class EnableBlockCommand: ICommand
    {
        private Unit unit;
        public EnableBlockCommand(Unit unit)
        {
            this.unit = unit;
        }
        public void Execute()
        {
            unit.Block();
        }

        public bool CanExecute()
        {
            return unit.CanBlock();
        }
    }
}