
using LineWars.Model;
using UnityEngine;

namespace LineWars.Model
{
    public class DefaultNation : INation
    {
        public GameObject GetUnitPrefab(UnitType type)
        {
            switch (type)
            {
                case UnitType.Infantry:
                    return Resources.Load<GameObject>("DefaultUnits/BaseInfrantry");
                default:
                    return Resources.Load<GameObject>("DefaultUnits/BaseInfrantry");
            }
        }
    }
}
