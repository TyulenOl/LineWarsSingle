namespace LineWars.Model
{
    public interface ICommandWithCommandType: ICommand
    {
        public CommandType CommandType { get; }
    }
}