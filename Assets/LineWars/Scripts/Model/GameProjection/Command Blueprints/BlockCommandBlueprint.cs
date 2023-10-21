namespace LineWars.Model
{
    public class BlockCommandBlueprint : ICommandBlueprint
    {
        public int UnitId { get; private set; }
        public BlockCommandBlueprint(int unitId)
        {
            UnitId = unitId;
        }

        public ICommand GenerateCommand(GameProjection projection)
        {
            var unit = projection.UnitsIndexList[UnitId];

            return new BlockCommand
                <NodeProjection, EdgeProjection, UnitProjection,
                OwnedProjection, BasePlayerProjection>(unit);
        }

        public ICommand GenerateMonoCommand(GameProjection projection)
        {
            var unit = projection.UnitsIndexList[UnitId].Original;

            return new BlockCommand
                <Node, Edge, Unit,
                Owned, BasePlayer>(unit);
        }
    }
}
