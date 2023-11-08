namespace LineWars.Model
{
    public class MoveCommandBlueprint : ICommandBlueprint
    {
        public int ExecutorId { get; private set; }
        public int NodeId { get; private set; }
        public MoveCommandBlueprint(int unitId, int nodeId)
        {
            ExecutorId = unitId;
            NodeId = nodeId;
        }

        public ICommand GenerateCommand(GameProjection projection)
        {
            var unit = projection.UnitsIndexList[ExecutorId];
            var node = projection.NodesIndexList[NodeId];

            return new MoveCommand<NodeProjection, EdgeProjection, UnitProjection>(unit, node);
        }

        public ICommand GenerateMonoCommand(GameProjection projection)
        {
            var unit = projection.UnitsIndexList[ExecutorId].Original;
            var node = projection.NodesIndexList[NodeId].Original;

            return new MoveCommand<Node, Edge, Unit>(unit, node);
        }
    }
}
