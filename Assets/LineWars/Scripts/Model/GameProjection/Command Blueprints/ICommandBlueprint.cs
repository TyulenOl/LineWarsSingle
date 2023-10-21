namespace LineWars.Model
{
    public interface ICommandBlueprint
    {
        public ICommand GenerateCommand(GameProjection projection);
        public ICommand GenerateMonoCommand(GameProjection projection);


    }
}
