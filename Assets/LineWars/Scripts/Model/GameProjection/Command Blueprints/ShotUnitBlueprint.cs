namespace LineWars.Model
{
    public class ShotUnitBlueprint : ICommandBlueprint
    {
        public int ExecutorId { get; private set; }
        public int UnitTargetId { get; private set; }
        public int NodeTargetId { get; private set; }

        public ShotUnitBlueprint(int executorId, int unitTargetId, int nodeTargetId)
        {
            ExecutorId = executorId;
            UnitTargetId = unitTargetId;
            NodeTargetId = nodeTargetId;
        }

        public ICommand GenerateCommand(GameProjection projection)
        {
            var executor = projection.UnitsIndexList[ExecutorId];
            var unitTarget = projection.UnitsIndexList[UnitTargetId];
            var nodeTarget = projection.NodesIndexList[NodeTargetId];
            return new ShotUnitCommand<NodeProjection, EdgeProjection, UnitProjection>(executor, unitTarget, nodeTarget);
        }

        public ICommand GenerateMonoCommand(GameProjection projection)
        {
            var executor = projection.UnitsIndexList[ExecutorId].Original;
            var unitTarget = projection.UnitsIndexList[UnitTargetId].Original;
            var nodeTarget = projection.NodesIndexList[NodeTargetId].Original;
            return new ShotUnitCommand<Node, Edge, Unit>(executor, unitTarget, nodeTarget);
        }
    }
}
