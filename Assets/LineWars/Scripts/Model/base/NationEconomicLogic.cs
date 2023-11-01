using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{    
    [CreateAssetMenu(fileName = "new Nation Economic", menuName = "Data/Create Nation Economic", order = 50)]
    [System.Serializable]
    public class NationEconomicLogic : ScriptableObject, IReadOnlyCollection<UnitBuyPreset>
    {
        [SerializeField, NamedArray("name")] private List<UnitBuyPreset> unitBuyPresets;
        public int Count => unitBuyPresets.Count;
        public IEnumerator<UnitBuyPreset> GetEnumerator() => unitBuyPresets.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => unitBuyPresets.GetEnumerator();
    }
}