using System;
using System.Linq;
using LineWars.Extensions.Attributes;
using LineWars.Interface;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Model
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Outline2D))]
    [RequireComponent(typeof(Node))]
    public class NodeInitialInfo : MonoBehaviour
    {
        [Header("Настройки принадлежности")]
        [SerializeField] private bool isSpawn;
        //or
        [SerializeField] private NodeInitialInfo referenceToSpawn;
        
        [Header("Настройки юнитов. Внимание! Левый юнит будет создаваться в приоритете")]
        [SerializeField] private Unit leftUnitPrefab;
        [SerializeField] private Unit rightUnitPrefab;

        [Header("Debug")] 
        [SerializeField] private Color groupColor = Color.white;

        public bool IsSpawn => isSpawn;
        public NodeInitialInfo ReferenceToSpawn => referenceToSpawn;
        public Unit LeftUnitPrefab => leftUnitPrefab;
        public Unit RightUnitPrefab => rightUnitPrefab;
        public Color GroupColor => groupColor;
        
        public void Initialize
        (
            bool isSpawn,
            NodeInitialInfo referenceToSpawn,
            Unit leftUnitPrefab,
            Unit rightUnitPrefab,
            Color groupColor
        )
        {
            this.isSpawn = isSpawn;
            this.referenceToSpawn = referenceToSpawn;
            this.leftUnitPrefab = leftUnitPrefab;
            this.rightUnitPrefab = rightUnitPrefab;
            this.groupColor = groupColor;
            RedrawColor();
        }      

        private void OnValidate()
        {
            RedrawColor();
        }

        private void RedrawColor()
        {
            if (isSpawn)
            {
                GetComponent<Outline2D>().SetActiveOutline(true);
                GetComponent<SpriteRenderer>().color = groupColor;
                foreach (var nodeInitialInfo in FindObjectsOfType<NodeInitialInfo>()
                             .Where(x => x.ReferenceToSpawn == this))
                {
                    nodeInitialInfo.RedrawColor();
                }
            }
            else if (referenceToSpawn != null)
            {
                GetComponent<Outline2D>().SetActiveOutline(false);
                GetComponent<SpriteRenderer>().color = referenceToSpawn.GroupColor;
            }
        }
    }
}