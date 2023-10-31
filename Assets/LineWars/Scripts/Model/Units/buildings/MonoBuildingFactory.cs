using System;

namespace LineWars.Model
{
    public class MonoBuildingFactory: IBuildingFactory
    {
        public IBuilding Create(BuildingType type)
        {
            switch (type)
            {
                case BuildingType.Fortification:
                    return default; // TODO
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}