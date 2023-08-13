using System;
using System.Linq;
using LineWars.Interface;
using UnityEngine;

namespace LineWars.Model
{
    [RequireComponent(typeof(Node))]
    [RequireComponent(typeof(NodeInitialInfo))]
    public class Spawn : MonoBehaviour
    {
        private static readonly string defaultName = "";
        private static readonly Color defaultColor = Color.white;

        [field: SerializeField] public string groupName { get; set; } = defaultName;
        [field: SerializeField] public Color groupColor { get; set; } = defaultColor;
        
        [field: SerializeField, HideInInspector] public Node Node { get; private set; }
        [field: SerializeField, HideInInspector] public NodeInitialInfo NodeInitialInfo { get; private set; }

        private void OnValidate()
        {
            AssignComponents();
            AssignFields();
            Redraw();
        }

        private void AssignComponents()
        {
            var referenceToSpawn = GetComponent<NodeInitialInfo>().ReferenceToSpawn;
            if (referenceToSpawn == null || referenceToSpawn != this)
                GetComponent<NodeInitialInfo>().ReferenceToSpawn = this;
        }

        private void AssignFields()
        {
            if (Node == null)
                Node = GetComponent<Node>();
            if (NodeInitialInfo == null)
                NodeInitialInfo = GetComponent<NodeInitialInfo>();
        }

        private void Redraw()
        {
            foreach (var info in FindObjectsOfType<NodeInitialInfo>()
                         .Where(x => x.ReferenceToSpawn == this))
                info.Redraw(this);
        }
    }
}