namespace LineWars.Model
{
    public class PerunActionCommandBlueprint : ICommandBlueprint
    {
        public int ExecutorId { get; private set; }
        public int NodeId { get; private set; } 

        public PerunActionCommandBlueprint(int executorId, int nodeId)
        {
            ExecutorId = executorId;
            NodeId = nodeId;
        }

        public ICommand GenerateCommand(GameProjection projection)
        {
            var unit = projection.UnitsIndexList[ExecutorId];
            var node = projection.NodesIndexList[NodeId];
            return new TargetedUniversalCommand
                <UnitProjection, ISacrificeForPerunAction<NodeProjection, EdgeProjection, UnitProjection>, NodeProjection>
                (unit, node);
        }

        public ICommand GenerateMonoCommand(GameProjection projection)
        {
            var unit = projection.UnitsIndexList[ExecutorId].Original;
            var node = projection.NodesIndexList[NodeId].Original;
            return new TargetedUniversalCommand
                <Unit, MonoSacrificeForPerunAction, Node>
                (unit, node);
        }
    }
}
