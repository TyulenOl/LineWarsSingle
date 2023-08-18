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
            unit.CanBlock = true;
        }

        public bool CanExecute()
        {
            return true;
        }
    }
}