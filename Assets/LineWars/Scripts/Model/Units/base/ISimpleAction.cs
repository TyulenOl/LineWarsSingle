namespace LineWars.Model
{
    public interface ISimpleAction
    {
        public ICommand GenerateCommand();
    }
}