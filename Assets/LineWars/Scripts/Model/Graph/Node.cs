using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Extensions.Attributes;
using LineWars.Interface;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LineWars.Model
{
    [RequireComponent(typeof(Selectable2D))]
    [RequireComponent(typeof(RenderNodeV3))]
    public class Node : Owned, INodeForGame<Node, Edge, Unit, Owned, BasePlayer>
    {
        [SerializeField] private int index;
        [SerializeField] private List<Edge> edges;

        //Если visibility равно 0, то видна только нода, если 1, то нода и ее соседи
        [SerializeField] [Min(0)] private int visibility;
        [SerializeField] [Min(0)] private int valueOfHidden;
        [SerializeField] private int baseIncome;

        [SerializeField, ReadOnlyInspector] private Unit leftUnit;
        [SerializeField, ReadOnlyInspector] private Unit rightUnit;

        [SerializeField] private Outline2D outline;
        [SerializeField] private Selectable2D selectable2D;
        [SerializeField] private RenderNodeV3 renderNodeV3;
        [SerializeField] private CommandPriorityData priorityData;
        
        [field: Header("Initial Info")]
        [field: SerializeField] public Spawn ReferenceToSpawn { get; set; }
        [field: SerializeField] public UnitType LeftUnitType { get; private set; }
        [field: SerializeField] public UnitType RightUnitType { get; private set; }
        

        private Camera mainCamera;
        
        /// <summary>
        /// Флаг, который указывает, что нода уже кому-то принадлежала
        /// </summary>
        public bool IsDirty { get; private set; }

        public bool IsBase => ReferenceToSpawn.Node == this;
        public Vector2 Position => transform.position;
        
        public IEnumerable<Edge> Edges => edges;

        public bool LeftIsFree => LeftUnit == null;
        public bool RightIsFree => RightUnit == null;
        public bool AnyIsFree => LeftIsFree || RightIsFree;
        public bool AllIsFree => LeftIsFree && RightIsFree;

        public int Id => index;

        public int Visibility =>
            Math.Max(visibility,
                Math.Max(
                    LeftUnit != null ? LeftUnit.Visibility : 0,
                    RightUnit != null ? RightUnit.Visibility : 0
                )
            );

        public int ValueOfHidden => valueOfHidden;

        public int BaseIncome => baseIncome;

        public Unit LeftUnit
        {
            get => leftUnit;
            set => leftUnit = value;
        }

        public Unit RightUnit
        {
            get => rightUnit;
            set => rightUnit = value;
        }
        
        public CommandPriorityData CommandPriorityData => priorityData;
        public RenderNodeV3 RenderNodeV3 => renderNodeV3;

        private void Awake()
        {
            mainCamera = Camera.main;
            IsDirty = ReferenceToSpawn != null;
        }
        
        private void Start()
        {
            var nodeInfoDrawer = GetComponent<NodeInfoDrawer>();
            nodeInfoDrawer.ReDrawCapturingInfo(Player.LocalPlayer.GetMyCapturingMoneyFromNode(this));
            nodeInfoDrawer.ReDrawIncomeInfo(Player.LocalPlayer.GetMyIncomeFromNode(this));
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

        protected void OnValidate()
        {
            if (outline == null)
                outline = GetComponent<Outline2D>();
            if (renderNodeV3 == null)
                renderNodeV3 = GetComponent<RenderNodeV3>();
            if (selectable2D == null)
                selectable2D = GetComponent<Selectable2D>();
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

            return gameObject;
        }

        public void Initialize(int index)
        {
            this.index = index;
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
            if (!IsDirty)
            {
                GetComponent<NodeInfoDrawer>().Capture();
            }
            ReferenceToSpawn = newPlayer != null ? basePlayer.Base.GetComponent<Spawn>() : null;
            IsDirty = true;
            Redraw();
        }
        
        
        #region Visualisation
        public void Redraw()
        {
            if (ReferenceToSpawn == null)
            {
                DrawToDefault();
            }
            else if (IsBase)
            {
                gameObject.name = $"Spawn {ReferenceToSpawn.GroupName}";
                var spriteRenderer = GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = ReferenceToSpawn.GroupSprite;
                GetComponent<Outline2D>().SetActiveOutline(true);
            }
            else
            {
                gameObject.name = $"Node{Id} Group with {ReferenceToSpawn.GroupName}";
                var spriteRenderer = GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = ReferenceToSpawn.GroupSprite;
                
            }
        }
        
        private void DrawToDefault()
        {
            gameObject.name = $"Node{Id}";
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Circle");
            GetComponent<Outline2D>().SetActiveOutline(false);
        }

        #endregion
    }
}