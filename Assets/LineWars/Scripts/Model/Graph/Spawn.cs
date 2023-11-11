using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    [RequireComponent(typeof(Node))]
    public class Spawn : MonoBehaviour
    {
        public const string DEFAULT_NAME = "";

        [field: Header("Logic")]
        [field: SerializeField] public PlayerRules Rules { get; private set; }
        [field: SerializeField] public Nation Nation { get; private set; }

        [field: Header("DEBUG")]
        [field: SerializeField] public string GroupName { get; set; } = DEFAULT_NAME;
        [field: SerializeField] public Sprite GroupSprite { get; set; }
        [field: SerializeField, HideInInspector] public Node Node { get; private set; }

        private void OnValidate()
        {
            if(GroupSprite == null && Nation != null)
            {
                GroupSprite = Nation.NodeSprite;
            }
            if ((GroupName == null || GroupName == "" || GroupName == string.Empty)
                && Nation != null)
            {
                GroupName = Nation.Name;
            }
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