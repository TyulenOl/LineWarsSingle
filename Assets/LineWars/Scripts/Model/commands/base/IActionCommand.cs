namespace LineWars.Model
{
    public interface IActionCommand: ICommand
    {
        public CommandType CommandType { get; }
        public ActionType ActionType { get; }
    }
}