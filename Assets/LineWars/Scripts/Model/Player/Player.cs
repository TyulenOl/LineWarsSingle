using System.Collections.Generic;
using LineWars.Extensions.Attributes;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class Player : BasePlayer
    {
        public static Player LocalPlayer { get; private set; }
        [SerializeField, ReadOnlyInspector] private int index;
        [SerializeField, ReadOnlyInspector] private NationType nationType;
        
        private INation nation;

        public int Index
        {
            get => index;
            set => index = value;
        }
        
        void Awake()
        {
            LocalPlayer = this;
            nation = NationHelper.GetNation(nationType);
        }
        public GameObject GetUnitPrefab(UnitType unitType) => nation.GetUnitPrefab(unitType);
    }
}