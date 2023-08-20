using System;
using LineWars.Interface;
using UnityEngine;

namespace LineWars.Model
{
    [ExecuteInEditMode]
    public class NodeInitialInfo: MonoBehaviour
    {
        [field: SerializeField] public Spawn ReferenceToSpawn { get; set; }

        [field: Header("Настройки юнитов. Внимание! Левый юнит будет создаваться в приоритете")]
        [field: SerializeField] public UnitType LeftUnitType { get; set; }
        [field: SerializeField] public UnitType RightUnitType { get; set; }
        [field:SerializeField, HideInInspector] public Node ReferenceToNode { get; private set; }


        private void Awake()
        {
            if (ReferenceToSpawn == null)
            {
                Debug.Log($"{nameof(ReferenceToSpawn)} in null on {gameObject.name}");
                Destroy(this);
            }
        }

        private void OnDisable()
        {
            Redraw(null);
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