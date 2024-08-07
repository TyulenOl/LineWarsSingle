﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

// ReSharper disable Unity.NoNullPropagation

namespace LineWars.Model
{
    [RequireComponent(typeof(RenderNodeV3))]
    public class Node :
        Owned,
        INodeForGame<Node, Edge, Unit>,
        IPointerClickHandler,
        INumbered,
        IMonoTarget
    {
        [SerializeField] private int index;

        // задается в префабе
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private List<Edge> edges;

        //Если visibility равно 0, то видна только нода, если 1, то нода и ее соседи
        [SerializeField] [Min(0)] private int visibility;
        [SerializeField] [Min(0)] private int valueOfHidden;
        [SerializeField] private int baseIncome;

        [SerializeField, ReadOnlyInspector] private Unit leftUnit;
        [SerializeField, ReadOnlyInspector] private Unit rightUnit;

        [SerializeField] private RenderNodeV3 renderNodeV3;
        [SerializeField] private CommandPriorityData priorityData;

        [field: Header("Sprite Info")]
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        [field: Header("Initial Info")]
        [field: SerializeField] public UnitType LeftUnitType { get; set; }
        [field: SerializeField] public UnitType RightUnitType { get; set; }

        private Camera mainCamera;

        public event Action<Node, Unit> UnitAdded;
        public event Action<Node, Unit> UnitLeft;
        public event Action<Node, int> NodeBaseIncomeChanged;

        /// <summary>
        /// Флаг, который указывает, что нода уже кому-то принадлежала
        /// </summary>
        public bool IsDirty { get; set; }
        public bool IsBase { get; set; }

        public Vector2 Position => transform.position;

        public IEnumerable<Edge> Edges => edges;
        public int EdgesCount => edges.Count;

        public bool LeftIsFree => LeftUnit is null;
        public bool RightIsFree => RightUnit is null;
        public bool AnyIsFree => LeftIsFree || RightIsFree;
        public bool AllIsFree => LeftIsFree && RightIsFree;

        public int Id
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

        public bool IsVisible => RenderNodeV3.Visibility > 0;

        public int BaseIncome
        {
            get => baseIncome;
            set
            {
                baseIncome = Mathf.Max(0, value);
                NodeBaseIncomeChanged?.Invoke(this, baseIncome);
            }
        }

        public Unit LeftUnit
        {
            get => leftUnit;
            set
            {
                var prevUnit = leftUnit;
                leftUnit = value;
                if (prevUnit == leftUnit)
                    return;
                
                if(prevUnit != null && prevUnit.Size == UnitSize.Little)
                {
                    ExecuteUnitLeft(prevUnit);
                }
                if(leftUnit != null && leftUnit.Size == UnitSize.Little)
                {
                    ExecuteUnitAdded(leftUnit);
                }
            }

        }

        public Unit RightUnit
        {
            get => rightUnit;
            set
            {
                var prevUnit = rightUnit;
                rightUnit = value;
                if (prevUnit == rightUnit)
                    return;

                if (prevUnit != null)
                {
                    ExecuteUnitLeft(prevUnit);
                }
                if (rightUnit != null)
                {
                    ExecuteUnitAdded(rightUnit);
                }
            }
        }

        private void ExecuteUnitLeft(Unit unit)
        {
            StartCoroutine(StupidCoroutine(unit));
            IEnumerator StupidCoroutine(Unit unit)
            {
                yield return null;
                UnitLeft?.Invoke(this, unit);
            }
        }

        private void ExecuteUnitAdded(Unit unit)
        {
            StartCoroutine(StupidCoroutine(unit));
            IEnumerator StupidCoroutine(Unit unit)
            {
                yield return null;
                UnitAdded?.Invoke(this, unit);
            }
        }

        public IBuilding Building { get; set; }

        public CommandPriorityData CommandPriorityData => priorityData;
        public RenderNodeV3 RenderNodeV3 => renderNodeV3;


        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Start()
        {
            var nodeInfoDrawer = GetComponent<NodeInfoDrawer>();
            nodeInfoDrawer.ReDrawCapturingInfo(Player.LocalPlayer.GetMyCapturingMoneyFromNode(this));
            nodeInfoDrawer.ReDrawIncomeInfo(Player.LocalPlayer.GetMyIncomeFromNode(this));
            if (Owner != null)
                nodeInfoDrawer.Capture();
        }

        public Edge GetLine(Node node) => Edges.Intersect(node.Edges).FirstOrDefault();

        public IEnumerable<Unit> Units => new[] {LeftUnit, RightUnit}
            .Where(x => x != null)
            .Distinct();

        public void OnPointerClick(PointerEventData eventData)
        {
            var absolutePosition = mainCamera.ScreenToWorldPoint(eventData.position);
            var relativePosition = absolutePosition - transform.position;

            if (relativePosition.x > 0)
            {
                if (!AnyIsFree) // обе заняты
                {
                    Selector.SelectedObjects = new[] {rightUnit?.gameObject, gameObject}
                        .Where(x => x != null)
                        .Distinct()
                        .ToArray();
                }
                else
                {
                    Selector.SelectedObjects = new[] {rightUnit?.gameObject, gameObject, leftUnit?.gameObject}
                        .Where(x => x != null)
                        .Distinct()
                        .ToArray();
                }
            }
            else
            {
                if (!AnyIsFree) // обе заняты
                {
                    Selector.SelectedObjects = new[] {leftUnit?.gameObject, gameObject}
                        .Where(x => x != null)
                        .Distinct()
                        .ToArray();
                }
                else
                {
                    Selector.SelectedObjects = new[] {leftUnit?.gameObject, gameObject, rightUnit?.gameObject}
                        .Where(x => x != null)
                        .Distinct()
                        .ToArray();
                }
            }
        }

        public void Initialize(int index)
        {
            this.index = index;
            edges = new List<Edge>();
        }


        public void AddEdge(Edge edge)
        {
            if (edge == null || edges.Contains(edge))
                return;
            edges.Add(edge);
        }

        public bool RemoveEdge(Edge edge) => edges.Remove(edge);
        public bool ContainsEdge(Edge edge) => edges.Contains(edge);
        
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

        public bool CanOwnerMove(int ownerId)
        {
            if (!TryGetComponent<NewPlayerMoveBan>(out var moveBan))
                return true;

            return moveBan.CanPlayerMove(ownerId);
        }


        protected override void OnSetOwner(BasePlayer oldPlayer, BasePlayer newPlayer)
        {
            if (!IsDirty)
            {
                GetComponent<NodeInfoDrawer>().Capture();
            }
            
            IsDirty = true;
            if (newPlayer == null)
                return;
            Redraw(IsBase, newPlayer.Nation.Name, newPlayer.Nation.NodeSprite);
        }

        public bool UnitIsFit(Unit unit)
        {
            return unit.Size == UnitSize.Large && AllIsFree 
                   || unit.Size == UnitSize.Little && AnyIsFree;
        }

        #region Visualisation
        
        public void Redraw(bool isSpawn, string groupName, Sprite sprite)
        {
            if (isSpawn)
            {
                gameObject.name = $"Spawn{Id} {groupName}";
            }
            else
            {
                gameObject.name = $"Node{Id} group with {groupName}";
            }
            spriteRenderer.sprite = sprite;
        }
        
        public void DrawToDefault()
        {
            gameObject.name = $"Node{Id}";
            spriteRenderer.sprite = defaultSprite;
        }
        
        public void Redraw()
        {
            gameObject.name = $"Node{Id}";
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (renderNodeV3 == null)
                renderNodeV3 = GetComponent<RenderNodeV3>();
            if (defaultSprite == null)
                Debug.LogError("Нет дефолтного спрайта у ноды", gameObject);
        }
#endif

        #endregion
    }
}