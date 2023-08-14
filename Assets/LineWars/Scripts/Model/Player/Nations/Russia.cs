
using System;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Model
{
    public class Russia : INation
    {
        public Unit GetUnitPrefab(UnitType type)
        {
            switch (type)
            {
                case UnitType.Infantry:
                    return Resources.Load<Unit>("Units/Russia/Infrantry");
                default:
                    return null;
            }
        }
    }
}
