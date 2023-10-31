namespace LineWars.Model
{
    public interface IBuildingFactory
    {
        public IBuilding Create(BuildingType type);
    }
}