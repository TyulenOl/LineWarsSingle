namespace LineWars.Model
{
    public interface IActionCommand : ICommand
    {
        public IExecutorAction Action { get; }

        public CommandType CommandType => Action.CommandType;
    }

    public interface IActionCommand<out TAction> : IActionCommand
        where TAction : IExecutorAction
    {
        public new TAction Action { get; }

        IExecutorAction IActionCommand.Action => Action;
    }
}