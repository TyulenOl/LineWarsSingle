using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// ReSharper disable Unity.NoNullPropagation

namespace LineWars.Model
{
    /// <summary>
    /// класс для создания плеера
    /// </summary>
    [RequireComponent(typeof(Node))]
    public class PlayerBuilder : MonoBehaviour
    {
        [field: Header("Logic")]
        [field: SerializeField] public PlayerRules Rules { get; private set; }

        [field: SerializeField] public Nation Nation { get; private set; }
        [field: SerializeField] public List<Node> InitialSpawns { get; private set; }

        [field: Header("DEBUG")]
        [field: SerializeField, HideInInspector] public Node Node { get; private set; }

        public string GroupName => Nation?.Name ?? "Default";
        public Sprite GroupSprite => Nation?.NodeSprite;

        private void OnValidate()
        {
            AssignFields();
            AssignComponents();
            Validate();
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

        private void Validate()
        {
            if (Rules == null)
                Debug.LogWarning($"У {nameof(PlayerBuilder)}({name}) нет {nameof(Rules)}");
            if (Nation == null)
                Debug.LogWarning($"У {nameof(PlayerBuilder)}({name}) нет {nameof(Nation)}");
        }

        private void Redraw()
        {
            foreach (var node in GetMyNodes())
                node.Redraw();
        }

        private IEnumerable<Node> GetMyNodes()
        {
            return FindObjectsOfType<Node>()
                .Where(x => x.ReferenceToSpawn == this);
        }
    }
}