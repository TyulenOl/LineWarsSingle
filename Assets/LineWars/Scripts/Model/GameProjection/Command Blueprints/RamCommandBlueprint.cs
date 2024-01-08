namespace LineWars.Model
{
    public class RamCommandBlueprint : ICommandBlueprint
    {
        public int ExecutorId {get; private set;}
        public int NodeId { get; private set;}

        public RamCommandBlueprint(int executorId, int nodeId)
        {
            ExecutorId = executorId;
            NodeId = nodeId;
        }

        public ICommand GenerateCommand(GameProjection projection)
        {
            var executor = projection.UnitsIndexList[ExecutorId];
            var node = projection.NodesIndexList[NodeId];

            return new TargetedUniversalCommand
                <UnitProjection, IRamAction<NodeProjection, EdgeProjection, UnitProjection>, NodeProjection>
                (executor, node);
        }

        public ICommand GenerateMonoCommand(GameProjection projection)
        {
            var executor = projection.UnitsIndexList[ExecutorId].Original;
            var node = projection.NodesIndexList[NodeId].Original;

            return new TargetedUniversalCommand
                <Unit, MonoRamAction, Node>
                (executor, node);
        }
    }
}