using LineWars.Extensions.Attributes;
using LineWars.Interface;
using UnityEngine;

namespace LineWars.Model
{
    public class Edge : MonoBehaviour, IAlive, IHitHandler
    {
        [SerializeField, ReadOnlyInspector] private int index;

        [SerializeField] [ReadOnlyInspector] private Node firstNode;
        [SerializeField] [ReadOnlyInspector] private Node secondNode;

        [SerializeField] [Min(1)] private int maxHp;
        [SerializeField] [ReadOnlyInspector] private int hp;
        [SerializeField] private LineType lineType;

        [SerializeField] [HideInInspector] private LineDrawer drawer;
        
        public Node FirsNode => firstNode;
        public Node SecondNode => secondNode;
        public LineDrawer Drawer => drawer;
        
        public int Hp => hp;
        
        public int Index
        {
            get => index;
            set => index = value;
        }

        public LineType LineType
        {
            get => lineType;
            set => lineType = value;
        }

        protected void OnValidate()
        {
            hp = maxHp;
        }

        public void Initialize(Node firstNode, Node secondNode)
        {
            this.firstNode = firstNode;
            this.secondNode = secondNode;
            drawer = GetComponent<LineDrawer>();
            drawer.Initialise(firstNode.transform, secondNode.transform);
        }

        public void Accept(Hit hit)
        {
            //TODO:Реализовать
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