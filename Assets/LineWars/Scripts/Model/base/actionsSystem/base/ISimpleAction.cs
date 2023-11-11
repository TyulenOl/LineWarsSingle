namespace LineWars.Model
{
    public interface ISimpleAction : IExecutorAction
    {
        public bool CanExecute();
        public void Execute();
        public IActionCommand GenerateCommand();
    }
}