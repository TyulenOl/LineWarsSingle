using UnityEngine;

namespace LineWars.Model
{
    public interface INation
    {
        public GameObject GetUnitPrefab(UnitType type);
    }
}
