using System.Collections.Generic;
using System.Linq;
using LineWars.Extensions.Attributes;
using LineWars.Interface;
using UnityEngine;

namespace LineWars.Model
{
    public class Node : Owned
    {
        [SerializeField, ReadOnlyInspector] private int index;
        [SerializeField] [HideInInspector] private List<Edge> edges;
        
        
        [SerializeField, ReadOnlyInspector] private Unit leftUnit;
        [SerializeField, ReadOnlyInspector] private Unit rightUnit;
        //[SerializeField] private bool isSpawn;
        
        [SerializeField] [HideInInspector] private Outline2D outline;
        
        //private Unit selectedUnit;
        private Camera mainCamera;
        
        public Vector2 Position => transform.position;
        public IReadOnlyCollection<Edge> Edges => edges;
        public bool LeftIsFree => LeftUnit == null;
        public bool RightIsFree => RightUnit == null;

        public int Hp => (LeftUnit ? LeftUnit.Hp : 0) + (RightUnit ? RightUnit.Hp : 0); 

        public int Index
        {
            get => index;
            set => index = value;
        }
        
        public Unit LeftUnit
        {
            get => leftUnit;
            set
            {
                //selectedUnit = null;
                leftUnit = value;
            }
        }

        public Unit RightUnit
        {
            get => rightUnit;
            set
            {
                //selectedUnit = null;
                rightUnit = value;
            }
        }

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void OnValidate()
        {
            //RedrawColor();
        }

        public void Initialize()
        {
            edges = new List<Edge>();
            outline = GetComponent<Outline2D>();
        }

        public void BeforeDestroy(out List<Edge> deletedEdges, out List<Node> neighbors)
        {
            neighbors = new List<Node>();
            deletedEdges = edges.ToList();

            foreach (var edge in edges)
            {
                var first = edge.FirsNode;
                var second = edge.SecondNode;
                if (first.Equals(this))
                {
                    second.RemoveEdge(edge);
                    neighbors.Add(second);
                }
                else
                {
                    first.RemoveEdge(edge);
                    neighbors.Add(first);
                }
            }

            edges = null;
        }
        
        public void AddEdge(Edge edge)
        {
            if (edge == null || edges.Contains(edge))
                return;
            edges.Add(edge);
        }

        public bool RemoveEdge(Edge edge) => edges.Remove(edge);

        public bool ContainsEdge(Edge edge) => edges.Contains(edge);

        public void SetActiveOutline(bool value) => outline.SetActiveOutline(value);
        
        // public void OnPointerClick(PointerEventData eventData)
        // {
        //     var mousePos = mainCamera.ScreenToWorldPoint(eventData.position);
        //     var relativeMousePos = mousePos - transform.position;
        //
        //     if (relativeMousePos.x < 0)
        //         selectedUnit = leftUnit;
        //     else
        //         selectedUnit = rightUnit;
        // }

        public IEnumerable<Node> GetNeighbors()
        {
            foreach (var edge in Edges)
            {
                if (edge.FirsNode.Equals(this))
                    yield return edge.SecondNode;
                else
                    yield return edge.FirsNode;
            }
        }
    }
}