namespace LineWars.Model
{
    public class RlBlockCommandBlueprint : ICommandBlueprint
    {
        public int ExecutorId { get; private set; }

        public RlBlockCommandBlueprint(int executorId)
        {
            ExecutorId = executorId;
        }

        public ICommand GenerateCommand(GameProjection projection)
        {
            var unit = projection.UnitsIndexList[ExecutorId];

            return new RLBlockCommand<NodeProjection, EdgeProjection, UnitProjection>(unit);
        }

        public ICommand GenerateMonoCommand(GameProjection projection)
        {
            var unit = projection.UnitsIndexList[ExecutorId].Original;

            return new RLBlockCommand<Node, Edge, Unit>(unit);
        }
    }
}
