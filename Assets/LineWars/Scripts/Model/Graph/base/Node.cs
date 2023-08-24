using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Extensions.Attributes;
using LineWars.Interface;
using UnityEngine;
using UnityEngine.EventSystems;
using LineWars.Controllers;

namespace LineWars.Model
{
    [RequireComponent(typeof(Outline2D))]
    [RequireComponent(typeof(Selectable2D))]
    public class Node : Owned, ITarget, INumbered
    {
        [SerializeField] private int index;
        [SerializeField] private List<Edge> edges;

        [SerializeField] [Min(0)] private int visibility;
        [SerializeField] [Min(0)] private int valueOfHidden;

        [SerializeField, ReadOnlyInspector] private Unit leftUnit;
        [SerializeField, ReadOnlyInspector] private Unit rightUnit;

        [SerializeField] private Outline2D outline;
        [SerializeField] private Selectable2D selectable2D;
        [SerializeField] private CommandPriorityData priorityData;

        [field: SerializeField] public bool IsBase { get; set; }

        private Camera mainCamera;

        public Vector2 Position => transform.position;
        public IReadOnlyCollection<Edge> Edges => edges;
        public bool LeftIsFree => LeftUnit == null;
        public bool RightIsFree => RightUnit == null;
        public bool AnyIsFree => LeftIsFree || RightIsFree;
        public bool AllIsFree => LeftIsFree && RightIsFree;

        public int Index
        {
            get => index;
            set => index = value;
        }

        public int Visibility =>
            Math.Max(visibility,
                Math.Max(
                    LeftUnit != null ? LeftUnit.Visibility : 0,
                    RightUnit != null ? RightUnit.Visibility : 0
                )
            );

        public int ValueOfHidden => valueOfHidden;

        public Unit LeftUnit
        {
            get => leftUnit;
            set { leftUnit = value; }
        }

        public Unit RightUnit
        {
            get => rightUnit;
            set { rightUnit = value; }
        }
        public CommandPriorityData CommandPriorityData => priorityData;

        private void Awake()
        {
            mainCamera = Camera.main;
        }


        private void OnEnable()
        {
            selectable2D.PointerClicked += OnPointerClicked;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            selectable2D.PointerClicked -= OnPointerClicked;
        }

        private GameObject OnPointerClicked(GameObject obj, PointerEventData eventData)
        {
            var absolutePosition = mainCamera.ScreenToWorldPoint(eventData.position);
            var relativePosition = absolutePosition - transform.position;

            if (relativePosition.x > 0)
            {
                if(rightUnit != null)
                    return rightUnit.gameObject;
            }
            else if (leftUnit != null)
            {
                return leftUnit.gameObject;
            }

            return this.gameObject;
        }

        public void Initialize()
        {
            edges = new List<Edge>();
        }

        public void BeforeDestroy(out List<Edge> deletedEdges, out List<Node> neighbors)
        {
            neighbors = new List<Node>();
            deletedEdges = edges.ToList();

            foreach (var edge in edges)
            {
                var first = edge.FirstNode;
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

        public IEnumerable<Node> GetNeighbors()
        {
            foreach (var edge in Edges)
            {
                if (edge.FirstNode.Equals(this))
                    yield return edge.SecondNode;
                else
                    yield return edge.FirstNode;
            }
        }

        protected override void OnSetOwner(BasePlayer oldPlayer, BasePlayer newPlayer)
        {
            if (newPlayer != null)
                Redraw(newPlayer.SpawnInfo.SpawnNode);
            else
                DrawToDefault();
        }

        #region Debug
        public void Redraw(Spawn spawn)
        {
            if (spawn == null)
            {
                DrawToDefault();
            }
            else if (GetComponent<Spawn>() != spawn)
            {
                gameObject.name = $"Node{Index} Group with {spawn.groupName}";
                GetComponent<SpriteRenderer>().color = spawn.groupColor;
            }
            else
            {
                gameObject.name = $"Spawn {spawn.groupName}";
                GetComponent<SpriteRenderer>().color = spawn.groupColor;
                GetComponent<Outline2D>().SetActiveOutline(true);
            }
        }


        public void DrawToDefault()
        {
            gameObject.name = $"Node{Index}";
            GetComponent<SpriteRenderer>().color = Spawn.defaultColor;
            GetComponent<Outline2D>().SetActiveOutline(false);
        }
        #endregion
    }
}