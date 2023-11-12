namespace LineWars.Model
{
    public interface IActionCommand<out TAction> : IActionCommand
        where TAction : IExecutorAction
    {
        public new TAction Action { get; }
        IExecutorAction IActionCommand.Action => Action;
    }
    
    public interface IActionCommand : ICommand
    {
        public IExecutorAction Action { get; }

        public CommandType CommandType => Action.CommandType;
        public ActionType ActionType => Action.ActionType;
    }
}