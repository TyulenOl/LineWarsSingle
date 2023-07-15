using System.Collections.Generic;
using LineWars;
using LineWars.Extensions.Attributes;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class Player : MonoBehaviour
    {
        public static Player LocalPlayer { get; private set; }
        [SerializeField, ReadOnlyInspector] private NationType nationType;

        private HashSet<Owned> myOwned = new();

        private INation nation;
        
        void Awake()
        {
        }

        private void Start()
        {
            LocalPlayer = this;
        }
       
        public bool IsMyOwn(Owned owned) => myOwned.Contains(owned);

        public void AddOwned(Owned owned)
        {
            if (owned != null)
                myOwned.Add(owned);
        }

        public void RemoveOwned(Owned owned)
        {
            myOwned.Remove(owned);
        }
        
        public GameObject GetUnitPrefab(UnitType unitType) => nation.GetUnitPrefab(unitType);
    }
}