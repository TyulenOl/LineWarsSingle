namespace LineWars.Model
{
    public class BlockCommandBlueprint : ICommandBlueprint
    {
        public int ExecutorId { get; private set; }
        public BlockCommandBlueprint(int unitId)
        {
            ExecutorId = unitId;
        }

        public ICommand GenerateCommand(GameProjection projection)
        {
            var unit = projection.UnitsIndexList[ExecutorId];

            return new BlockCommand<NodeProjection, EdgeProjection, UnitProjection>(unit);
        }

        public ICommand GenerateMonoCommand(GameProjection projection)
        {
            var unit = projection.UnitsIndexList[ExecutorId].Original;

            return new BlockCommand<Node, Edge, Unit>(unit);
        }
    }
}
