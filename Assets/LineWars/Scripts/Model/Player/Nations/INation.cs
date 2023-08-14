using UnityEngine;

namespace LineWars.Model
{
    public interface INation
    {
        public Unit GetUnitPrefab(UnitType type);
    }
}
