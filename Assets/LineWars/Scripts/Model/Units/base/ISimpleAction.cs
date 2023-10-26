namespace LineWars.Model
{
    public interface ISimpleAction
    {
        public ICommandWithCommandType GenerateCommand();
    }
}