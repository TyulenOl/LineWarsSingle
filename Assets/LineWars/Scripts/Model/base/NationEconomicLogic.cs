using System.Collections.Generic;
using LineWars.Extensions.Attributes;
using UnityEngine;

namespace LineWars.Model
{    
    [CreateAssetMenu(fileName = "new Nation Economic", menuName = "Data/Create Nation Economic", order = 50)]
    [System.Serializable]
    public class NationEconomicLogic : ScriptableObject
    {
        [SerializeField, NamedArray("name")] private List<UnitBuyPreset> unitBuyPresets;

        public List<UnitBuyPreset> UnitBuyPresets => unitBuyPresets;
    }
}