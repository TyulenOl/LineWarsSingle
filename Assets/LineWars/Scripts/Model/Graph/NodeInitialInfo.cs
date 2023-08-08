using System;
using LineWars.Interface;
using UnityEngine;

namespace LineWars.Model
{
    public class NodeInitialInfo: MonoBehaviour
    {
        private static readonly Color defaultColor = Color.white;
        
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

        public void Redraw(Spawn spawn)
        {
            if (spawn == null)
            {
                gameObject.name = $"Node{ReferenceToNode.Index}";
                GetComponent<SpriteRenderer>().color = defaultColor;
                GetComponent<Outline2D>().SetActiveOutline(false);
            }
            else if (GetComponent<Spawn>() != spawn)
            {
                gameObject.name = $"Node{ReferenceToNode.Index} Group with {spawn.groupName}";
                GetComponent<SpriteRenderer>().color = spawn.groupColor;
            }
            else
            {
                gameObject.name = $"Spawn {spawn.groupName}";
                GetComponent<SpriteRenderer>().color = spawn.groupColor;
                GetComponent<Outline2D>().SetActiveOutline(true);
            }
        }
    }
}