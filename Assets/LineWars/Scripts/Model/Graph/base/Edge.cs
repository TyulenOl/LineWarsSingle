using LineWars.Extensions.Attributes;
using LineWars.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Model
{
    public class Edge : MonoBehaviour, IAlive, ITarget, INumbered
    {
        [Header("Graph Settings")]
        [SerializeField] private int index;
        [SerializeField] [ReadOnlyInspector] private Node firstNode;
        [SerializeField] [ReadOnlyInspector] private Node secondNode;

        [Header("Logical Settings")]
        [SerializeField] [Min(1)] private int maxHp = 2;
        [SerializeField] [ReadOnlyInspector] private int hp;
        [SerializeField] private LineType lineType;

        [Header("Commands Settings")]
        [SerializeField] private CommandPriorityData priorityData;


        [SerializeField] [HideInInspector] private LineDrawer drawer;


        [field:Header("Events")] 
        [field: SerializeField] public UnityEvent<int, int> HpChanged { get; private set; }
        [field: SerializeField] public UnityEvent<LineType, LineType> LineTypeChanged { get; private set; }
        [field: SerializeField] public UnityEvent<Unit> Died { get; private set; }

        public int Index
        {
            get => index;
            set => index = value;
        }
        public Node FirstNode => firstNode;
        public Node SecondNode => secondNode;
        public LineDrawer Drawer => drawer;
        
        public int CurrentHp
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
        public bool IsDied => CurrentHp <= 0;
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

        public CommandPriorityData CommandPriorityData => priorityData;
        
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
            if (FirstNode.Equals(node))
                return SecondNode;
            else
                return FirstNode;
        }
    }
}