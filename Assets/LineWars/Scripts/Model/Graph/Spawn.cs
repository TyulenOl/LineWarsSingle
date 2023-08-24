using System;
using System.Linq;
using LineWars.Interface;
using UnityEngine;

namespace LineWars.Model
{
    [RequireComponent(typeof(Node))]
    public class Spawn : MonoBehaviour
    {
        public static readonly string defaultName = "";
        public static readonly Color defaultColor = Color.white;

        [field: SerializeField] public NationType Nation { get; private set; }
        [field: SerializeField] public int StartMoney { get; private set; }
        [field: SerializeField] public string groupName { get; set; } = defaultName;
        [field: SerializeField] public Color groupColor { get; set; } = defaultColor;

        [field: SerializeField, HideInInspector] public Node Node { get; private set; }

        private void OnValidate()
        {
            AssignFields();
            AssignComponents();
            Redraw();
        }

        private void AssignComponents()
        {
            Node.ReferenceToSpawn = this;
        }

        private void AssignFields()
        {
            if (Node == null)
                Node = GetComponent<Node>();
        }

        private void Redraw()
        {
            foreach (var info in FindObjectsOfType<Node>().Where(x => x.ReferenceToSpawn == this))
                info.Redraw();
        }
    }
}