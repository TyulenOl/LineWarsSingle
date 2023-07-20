using System;
using LineWars.Extensions.Attributes;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Model
{
    [RequireComponent(typeof(Node))]
    public class NodeInitialInfo : MonoBehaviour
    {
        [SerializeField] private bool isSpawn;
        //or
        [SerializeField] private NodeInitialInfo referenceToSpawn;

        [SerializeField] private Unit leftUnitPrefab;
        [SerializeField] private Unit rightUnitPrefab;

        public bool IsSpawn => isSpawn;
        public NodeInitialInfo ReferenceToSpawn => referenceToSpawn;
        public Unit LeftUnitPrefab => leftUnitPrefab;
        public Unit RightUnitPrefab => rightUnitPrefab;
        

#if UNITY_EDITOR
        public void Initialize
        (
            bool isSpawn,
            NodeInitialInfo referenceToSpawn,
            Unit leftUnitPrefab,
            Unit rightUnitPrefab
        )
        {
            this.isSpawn = isSpawn;
            this.referenceToSpawn = referenceToSpawn;
            this.leftUnitPrefab = leftUnitPrefab;
            this.rightUnitPrefab = rightUnitPrefab;
            RedrawColor();
        }      

        private void OnValidate()
        {
            RedrawColor();
        }
#endif
        private void RedrawColor() => GetComponent<SpriteRenderer>().color = isSpawn ? Color.green : Color.white;
    }
}