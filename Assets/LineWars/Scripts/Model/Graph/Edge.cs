using LineWars.Extensions.Attributes;
using LineWars.Interface;
using UnityEngine;

namespace LineWars.Model
{
    public class Edge: MonoBehaviour, IEdge
    {
        [SerializeField, ReadOnlyInspector] private int index; 
        
        [SerializeField] [ReadOnlyInspector] private Node firstNode;
        [SerializeField] [ReadOnlyInspector] private Node secondNode;
        [SerializeField] [ReadOnlyInspector] private LineDrawer drawer;
        public INode FirsNode => firstNode;
        public INode SecondNode => secondNode;
        public LineDrawer Drawer => drawer;

        public int Index
        {
            get => index;
            set => index = value;
        }

        public Node FirstNode
        {
            get => firstNode;
            set => firstNode = value;
        }

        public void Initialize(Node firstNode, Node secondNode)
        {
            this.firstNode = firstNode;
            this.secondNode = secondNode;
            drawer = GetComponent<LineDrawer>();
            drawer.Initialise(firstNode.transform, secondNode.transform);
        }

        public void ReDraw()
        {
            drawer.DrawLine();
        }

        public Node GetFirstNode() => firstNode;
        public Node GetSecondNode() => secondNode;
    }
}