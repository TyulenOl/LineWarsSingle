
using System;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Model
{
    public class DefaultNation : INation
    {
        public Unit GetUnitPrefab(UnitType type)
        {
            switch (type)
            {
                case UnitType.TheRifleMan:
                    return Resources.Load<Unit>("Units/DefaultUnits/BaseInfrantry");
                default:
                    return null;
            }
        }
    }
}
