
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
                case UnitType.TheRifleMan:
                    return Resources.Load<Unit>("Nations/Russia/Units/Infrantry");
                default:
                    return null;
            }
        }
    }
}
