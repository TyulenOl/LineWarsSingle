using LineWars.Extensions.Attributes;
using LineWars.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Model
{
    public class Edge : MonoBehaviour, IAlive
    {
        [Header("Graph Settings")]
        [SerializeField, ReadOnlyInspector] private int index;
        [SerializeField] [ReadOnlyInspector] private Node firstNode;
        [SerializeField] [ReadOnlyInspector] private Node secondNode;

        [Header("Logical Settings")]
        [SerializeField] [Min(1)] private int maxHp = 2;
        [SerializeField] [ReadOnlyInspector] private int hp;
        [SerializeField] private LineType lineType;

        [SerializeField] [HideInInspector] private LineDrawer drawer;

        [Header("Events")] 
        public UnityEvent<int, int> HpChanged;
        public UnityEvent<LineType, LineType> LineTypeChanged;

        public int Index
        {
            get => index;
            set => index = value;
        }
        public Node FirsNode => firstNode;
        public Node SecondNode => secondNode;
        public LineDrawer Drawer => drawer;
        
        public int Hp
        {
            get => hp;
            private set
            {
                var before = hp;

                if (value < 0)
                {
                    LineType = LineTypeHelper.Down(LineType);
                    hp = maxHp;
                }
                else 
                    hp = value;
                HpChanged.Invoke(before, hp);
            }
        }

        public LineType LineType
        {
            get => lineType;
            private set
            {
                var before = lineType;
                lineType = value;
                LineTypeChanged.Invoke(before, lineType);
            }
        }

        protected void OnValidate()
        {
            hp = maxHp;
        }

        public void Initialize(Node firstNode, Node secondNode, LineType lineType = LineType.ScoutRoad)
        {
            this.firstNode = firstNode;
            this.secondNode = secondNode;
            drawer = GetComponent<LineDrawer>();
            drawer.Initialise(firstNode.transform, secondNode.transform);
        }
        
        public void DealDamage(Hit hit)
        {
            throw new System.NotImplementedException();
        }
        
        public void ReDraw() => drawer.DrawLine();

        public Node GetOther(Node node)
        {
            if (FirsNode.Equals(node))
                return SecondNode;
            else
                return FirsNode;
        }
    }
}