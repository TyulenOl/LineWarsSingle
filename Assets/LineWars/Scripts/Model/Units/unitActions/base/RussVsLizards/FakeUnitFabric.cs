namespace LineWars.Model
{
    public class FakeUnitFabric : IUnitFabric<NodeProjection, EdgeProjection, UnitProjection>
    {
        public bool CanSpawn(NodeProjection node)
        {
            return false;
        }

        public UnitProjection Spawn(NodeProjection node)
        {
            return null;
        }
    }
}
