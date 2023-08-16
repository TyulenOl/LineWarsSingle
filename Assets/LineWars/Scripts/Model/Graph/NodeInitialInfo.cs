using System;
using LineWars.Interface;
using UnityEngine;

namespace LineWars.Model
{
    public class NodeInitialInfo: MonoBehaviour
    {
        [field: SerializeField] public Spawn ReferenceToSpawn { get; set; }

        [field: Header("Настройки юнитов. Внимание! Левый юнит будет создаваться в приоритете")]
        [field: SerializeField] public Unit LeftUnitPrefab { get; set; }
        [field: SerializeField] public Unit RightUnitPrefab { get; set; }
        [field:SerializeField, HideInInspector] public Node ReferenceToNode { get; private set; }


        private void Awake()
        {
            if (ReferenceToSpawn == null)
            {
                Debug.Log($"{nameof(ReferenceToSpawn)} in null on {gameObject.name}");
                Destroy(this);
            }
        }

        private void OnValidate()
        {
            AssignFields();
            Redraw(ReferenceToSpawn);
        }

        private void AssignFields()
        {
            if (ReferenceToNode == null)
                ReferenceToNode = GetComponent<Node>();
        }
        public void Redraw(Spawn referenceToSpawn)
        {
            ReferenceToNode.Redraw(referenceToSpawn);
        }
    }
}