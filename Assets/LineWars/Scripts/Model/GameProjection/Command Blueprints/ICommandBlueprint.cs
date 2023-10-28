namespace LineWars.Model
{
    public interface ICommandBlueprint
    {
        public int ExecutorId { get; }
        public ICommand GenerateCommand(GameProjection projection);
        public ICommand GenerateMonoCommand(GameProjection projection);


    }
}
