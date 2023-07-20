using System;
using UnityEngine;

namespace LineWars.Model
{
    [Serializable]
    public class AdditionalNodeData
    {
        [SerializeField] private int ownedIndex;
        [SerializeField] private bool isSpawn;
        [SerializeField] private Unit leftUnitPrefab;
        [SerializeField] private Unit rightUnitPrefab;

        public bool HasOwner => ownedIndex != -1;
        
        public AdditionalNodeData()
        {
            ownedIndex = -1;
            isSpawn = false;
            leftUnitPrefab = null;
            rightUnitPrefab = null;
        }
        public AdditionalNodeData(int ownedIndex, bool isSpawn, Unit leftUnitPrefab, Unit rightUnitPrefab)
        {
            this.ownedIndex = ownedIndex;
            this.isSpawn = isSpawn;
            this.leftUnitPrefab = leftUnitPrefab;
            this.rightUnitPrefab = rightUnitPrefab;
        }
        
        public int OwnedIndex => ownedIndex;
        public bool IsSpawn => isSpawn;
        public Unit LeftUnitPrefab => leftUnitPrefab;
        public Unit RightUnitPrefab => rightUnitPrefab;
    }
}