using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "new Nation", menuName = "Data/Create Nation", order = 50)]
    public class Nation: ScriptableObject
    {
        [SerializeField] private NationEconomicLogic nationEconomicLogic;
        [field:SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite NodeSprite { get; private set; }
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
        
        public Unit GetUnitPrefab(UnitType type)
        {
            if (type == UnitType.None)
                return null;
            if (UnitTypeUnitPairs.TryGetValue(type, out var unit))
            {
                if (unit == null)
                    Debug.LogWarning($"UnitPrefab is missing in {name} by key {type}", this);
                return unit;
            }
            Debug.LogWarning($"In nation {name} not found unit prefab by key {type}");
            return null;
        }
    }
}
