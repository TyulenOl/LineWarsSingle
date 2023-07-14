using System.Collections.Generic;
using System.Linq;
using LineWars.Extensions.Attributes;
using LineWars.Interface;
using UnityEngine;
using UnityEngine.Serialization;

namespace LineWars.Model
{
    public class Node : MonoBehaviour, INode
    {
        [SerializeField, ReadOnlyInspector] private int index;
        
        [SerializeField] private bool isSpawn;
        [SerializeField] [HideInInspector] private List<Edge> edges;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Outline2D outline;
        
        public Vector2 Position => transform.position;
        public IReadOnlyCollection<IEdge> Edges => edges;
        public IReadOnlyList<Edge> GetEdgesList() => edges;

        public int Index
        {
            get => index;
            set => index = value;
        }

        public bool IsSpawn
        {
            get => isSpawn;
            set
            {
                isSpawn = value;
                RedrawColor();
            }
        }
        
        protected void OnValidate()
        {
            RedrawColor();
        }

        public void Initialize()
        {
            edges = new List<Edge>();
        }
        
        private void RedrawColor()
        {
            if (isSpawn)
                spriteRenderer.color = Color.green;
            else
            {
                spriteRenderer.color = Color.white;
            }
        }

        public void BeforeDestroy(out List<Edge> deletedEdges, out List<Node> neighbors)
        {
            neighbors = new List<Node>();
            deletedEdges = edges.ToList();
            
            foreach (var edge in edges)
            {
                var first = edge.GetFirstNode();
                var second = edge.GetSecondNode();
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
            if (edges.Contains(edge))
                return;
            edges.Add(edge);
        }

        public bool RemoveEdge(Edge edge) => edges.Remove(edge);

        public void SetActiveOutline(bool value) => outline.SetActiveOutline(value);
    }
}