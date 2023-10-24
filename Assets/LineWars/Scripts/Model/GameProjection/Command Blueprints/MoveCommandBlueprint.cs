namespace LineWars.Model
{
    public class MoveCommandBlueprint : ICommandBlueprint
    {
        public int UnitId { get; private set; }
        public int NodeId { get; private set; }
        public MoveCommandBlueprint(int unitId, int nodeId)
        {
            UnitId = unitId;
            NodeId = nodeId;
        }

        public ICommand GenerateCommand(GameProjection projection)
        {
            var unit = projection.UnitsIndexList[UnitId];
            var node = projection.NodesIndexList[NodeId];

            return new MoveCommand
                <NodeProjection, EdgeProjection, UnitProjection, 
                OwnedProjection, BasePlayerProjection>
                (unit, node);
        }

        public ICommand GenerateMonoCommand(GameProjection projection)
        {
            var unit = projection.UnitsIndexList[UnitId].Original;
            var node = projection.NodesIndexList[NodeId].Original;

            return new MoveCommand<Node, Edge, Unit, Owned, BasePlayer>(unit, node);
        }
    }
}
