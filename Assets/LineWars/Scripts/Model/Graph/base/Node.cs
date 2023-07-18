using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Extensions;
using LineWars.Extensions.Attributes;
using LineWars.Interface;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using LineWars.Controllers;

namespace LineWars.Model
{
    
    public class Node : Owned
    {
        [SerializeField, ReadOnlyInspector] private int index;
        [SerializeField] [HideInInspector] private List<Edge> edges;
        
        
        [SerializeField, ReadOnlyInspector] private Unit leftUnit;
        [SerializeField, ReadOnlyInspector] private Unit rightUnit;
        [SerializeField] private bool isSpawn;

        [SerializeField] [HideInInspector] private SpriteRenderer spriteRenderer;
        [SerializeField] [HideInInspector] private Outline2D outline;
        [SerializeField] [HideInInspector] private Selectable2D selectable2D;
        
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

        public bool IsSpawn
        {
            get => isSpawn;
            set
            {
                isSpawn = value;
                RedrawColor();
            }
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
            RedrawColor();
        }

        private void OnEnable() 
        {
            selectable2D.PointerClicked += OnPointerClicked;
        }

        private void OnDisable() 
        {
            selectable2D.PointerClicked -= OnPointerClicked;
        }

        private GameObject OnPointerClicked(GameObject obj, PointerEventData eventData)
        {   
    
            var absolutePosition = mainCamera.ScreenToWorldPoint(eventData.position);
            var relativePosition = absolutePosition - transform.position;

            if(relativePosition.x > 0 && rightUnit != null)
            {
                return rightUnit.gameObject;
            }
            else if(leftUnit != null)
            {
                return leftUnit.gameObject;
            }
            return this.gameObject;
        }

        public void Initialize()
        {
            edges = new List<Edge>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            outline = GetComponent<Outline2D>();
            selectable2D = GetComponent<Selectable2D>();
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

        private void RedrawColor()
        {
            if (isSpawn)
                spriteRenderer.color = Color.green;
            else
            {
                spriteRenderer.color = Color.white;
            }
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