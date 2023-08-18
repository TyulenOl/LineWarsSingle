using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "new Nation", menuName = "Data/Create Nation", order = 50)]
    public class Nation: ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector] private List<UnitType> unitTypes = new();
        [SerializeField, HideInInspector] private List<Unit> units = new();
        public Dictionary<UnitType, Unit> UnitTypeUnitPairs { get; private set; } = new();

        private void OnEnable()
        {
            UpdateTypes();
        }

        private void UpdateTypes()
        {
            var allUnitTypes = Enum.GetValues(typeof(UnitType))
                .OfType<UnitType>()
                .Where(x => x is not UnitType.None)
                .ToArray();
            foreach (var unitType in allUnitTypes)
                UnitTypeUnitPairs.TryAdd(unitType, null);
        }

        public Unit GetUnitPrefab(UnitType type)
        {
            if (UnitTypeUnitPairs.TryGetValue(type, out var unit))
                return unit;
            return null;
        }
        
        public void OnBeforeSerialize()
        {
            unitTypes.Clear();
            units.Clear();
            foreach (var (unitType, unit)  in UnitTypeUnitPairs)
            {
                unitTypes.Add(unitType);
                units.Add(unit);
            }
        }

        public void OnAfterDeserialize()
        {
            UnitTypeUnitPairs = new Dictionary<UnitType, Unit>();

            for (int i = 0; i != Math.Min(unitTypes.Count, units.Count); i++)
                UnitTypeUnitPairs.Add(unitTypes[i], units[i]);
            
            UpdateTypes();
        }
    }
}
