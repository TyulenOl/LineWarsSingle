using System.Collections;
using System.Collections.Generic;
using LineWars.Extensions.Attributes;
using UnityEngine;

namespace LineWars.Model
{    
    [CreateAssetMenu(fileName = "new Nation Economic", menuName = "Data/Create Nation Economic", order = 50)]
    [System.Serializable]
    public class NationEconomicLogic : ScriptableObject, IReadOnlyCollection<UnitBuyPreset>
    {
        [SerializeField, NamedArray("name")] private List<UnitBuyPreset> unitBuyPresets;

        public int Count => ((IReadOnlyCollection<UnitBuyPreset>)unitBuyPresets).Count;

        public IEnumerator<UnitBuyPreset> GetEnumerator()
        {
            return ((IEnumerable<UnitBuyPreset>)unitBuyPresets).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)unitBuyPresets).GetEnumerator();
        }
    }
}