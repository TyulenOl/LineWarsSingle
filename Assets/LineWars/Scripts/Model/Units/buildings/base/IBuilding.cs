namespace LineWars.Model
{
    public interface IBuilding
    {
        public int NodeId { get; }
        public BuildingType BuildingType { get; }
    }
}