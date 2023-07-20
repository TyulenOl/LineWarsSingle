
using LineWars.Model;
using UnityEngine;

namespace LineWars.Model
{
    public class Russia : INation
    {
        public GameObject GetUnitPrefab(UnitType type)
        {
            switch (type)
            {
                case UnitType.Infantry:
                    return Resources.Load<GameObject>("Russia/Infrantry");
                default:
                    return Resources.Load<GameObject>("DefaultUnits/BaseInfrantry");
            }
        }
    }
}
