namespace LineWars.Model
{
    public interface IExecutor : IReadOnlyExecutor
    {
        public new int CurrentActionPoints { get; set; }
        public bool TryGetCommand(CommandType priorityType, ITarget target, out ICommand command);
    }
}