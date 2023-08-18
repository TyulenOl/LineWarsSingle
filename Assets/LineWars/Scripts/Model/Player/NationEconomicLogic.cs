using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Scripts.Interface.UnitBuyLogic
{    
    [CreateAssetMenu(fileName = "new Nation Economic", menuName = "Data/Create Nation Economic", order = 50)]
    [System.Serializable]
    public class NationEconomicLogic : ScriptableObject
    {
        [SerializeField] private List<UnitBuyPreset> unitBuyPresets;

        public List<UnitBuyPreset> UnitBuyPresets => unitBuyPresets;
    }
}