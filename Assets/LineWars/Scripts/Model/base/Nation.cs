using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "new Nation", menuName = "Data/Create Nation", order = 50)]
    [Serializable]
    public class Nation: ScriptableObject
    {
        [SerializeField] private NationEconomicLogic nationEconomicLogic;
        [field: SerializeField] public SerializedDictionary<UnitType, Unit> UnitTypeUnitPairs { get; private set; } = new();
        
        public NationEconomicLogic NationEconomicLogic => nationEconomicLogic;
        
        private void OnEnable()
        {
            ValidateUnitTypeUnitPairs();
        }
        
        private void ValidateUnitTypeUnitPairs()
        {
            foreach (var (key, value) in UnitTypeUnitPairs)
            {
                if (value == null)
                    return;
                if (key != value.Type)
                {
                    UnitTypeUnitPairs[key] = null;
                    Debug.LogWarning($"Ключь и значени типа юнита не совпадают! {name}");
                }
            }
        }
        
        public Unit GetUnit(UnitType type)
        {
            if (UnitTypeUnitPairs.TryGetValue(type, out var unit))
                return unit;
            return null;
        }
    }
}
