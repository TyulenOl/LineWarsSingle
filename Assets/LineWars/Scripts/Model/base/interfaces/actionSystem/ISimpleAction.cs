namespace LineWars.Model
{
    public interface ISimpleAction
    {
        public bool CanExecute();
        public void Execute();
        public IActionCommand GenerateCommand();
    }
}