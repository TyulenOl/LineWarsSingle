﻿// ReSharper disable ParameterHidesMember
using System;
using System.Collections.Generic;
using System.Linq;
using GraphEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace LineWars.Model
{
    public class Edge : MonoBehaviour,
        IEdgeForGame<Node, Edge, Unit>,
        ISerializationCallbackReceiver,
        IPointerClickHandler,
        INumbered,
        IMonoTarget
    {
        [Header("Graph Settings")]
        [SerializeField] private int index;

        [SerializeField] private Node firstNode;
        [SerializeField] private Node secondNode;

        [Header("Line Settings")] 
        [SerializeField] private LineType lineType;

        [SerializeField, NamedArray("lineType")]
        private List<LineTypeCharacteristics> lineTypeCharacteristics;

        [Header("Commands Settings")] 
        [SerializeField] private CommandPriorityData priorityData;

        [Header("DEBUG")] 
        [SerializeField] [ReadOnlyInspector] private int hp;

        [field: Header("Events")]
        [field: SerializeField] public UnityEvent<int, int> HpChanged { get; private set; }

        [field: SerializeField] public UnityEvent<LineType, LineType> LineTypeChanged { get; private set; }

        private Dictionary<LineType, LineTypeCharacteristics> lineMap;
        
        [Header("References")]
        [SerializeField] private SpriteRenderer edgeSpriteRenderer;
        [SerializeField] private BoxCollider2D edgeCollider;

        public IReadOnlyDictionary<LineType, LineTypeCharacteristics> LineMap => lineMap;
        public SpriteRenderer SpriteRenderer => edgeSpriteRenderer;
        public BoxCollider2D BoxCollider2D => edgeCollider;


        public int Id
        {
            get => index;
            set => index = value;
        }


        public int MaxHp
        {
            get => lineMap[LineType].MaxHp;
            set => lineMap[LineType].MaxHp = value;
        }

        public Node FirstNode
        {
            get => firstNode;
            set => firstNode = value;
        }

        public Node SecondNode
        {
            get => secondNode;
            set => secondNode = value;
        }

        public int CurrentHp
        {
            get => hp;
            set
            {
                var before = hp;

                if (value < 0)
                {
                    LineType = LineTypeHelper.Down(LineType);
                    hp = MaxHp;
                }
                else
                    hp = Math.Min(value, MaxHp);

                HpChanged.Invoke(before, hp);
            }
        }
        
        public LineType LineType
        {
            get => lineType;
            set
            {
                var before = lineType;
                lineType = value;
                LineTypeChanged.Invoke(before, lineType);
                CurrentHp = MaxHp;
            }
        }
        
        public CommandPriorityData CommandPriorityData => priorityData;

        public float GetCurrentWidth() => lineMap.TryGetValue(lineType, out var ch) 
            ? ch.Width 
            : 0.1f;

        public Sprite GetCurrentSprite() => lineMap.TryGetValue(lineType, out var ch)
            ? ch.Sprite
            : Resources.Load<Sprite>("Sprites/Road");


        protected void OnValidate()
        {
            hp = MaxHp;
#if UNITY_EDITOR
            if (EditorExtensions.CanRedraw(gameObject))
            {
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    if (gameObject != null && EditorExtensions.CanRedraw(gameObject))
                        Redraw();
                };
            }
#endif
        }
        
        public void Initialize(int index, Node firstNode, Node secondNode)
        {
            this.index = index;
            this.firstNode = firstNode;
            this.secondNode = secondNode;
        }
        
        public void LevelUp()
        {
            LineType = LineTypeHelper.Up(LineType);
        }
        
        public void OnBeforeSerialize()
        {
        }
        public void OnAfterDeserialize()
        {
            lineMap = new Dictionary<LineType, LineTypeCharacteristics>();

            for (int i = 0; i != lineTypeCharacteristics.Count; i++)
                lineMap.TryAdd(lineTypeCharacteristics[i].LineType, lineTypeCharacteristics[i]);

            UpdateTypes();
            
            void UpdateTypes()
            {
                foreach (var value in Enum.GetValues(typeof(LineType)).OfType<LineType>())
                {
                    if (!lineMap.ContainsKey(value))
                        lineMap[value] = new LineTypeCharacteristics(value);
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Selector.SelectedObject = gameObject;
        }
        
        public void Redraw()
        {
            name = $"Edge{Id}";
            RedrawLine();
            AlineCollider();
        
            void RedrawLine()
            {
                var v1 = FirstNode ? FirstNode.Position : Vector2.zero;
                var v2 = SecondNode ? SecondNode.Position : Vector2.right;
                var distance = Vector2.Distance(v1, v2);
                var center = v1;
                var newSecondNodePosition = v2 - center;
                var radian = Mathf.Atan2(newSecondNodePosition.y, newSecondNodePosition.x) * 180 / Mathf.PI;
            
         
                SpriteRenderer.transform.rotation = Quaternion.Euler(0, 0, radian);
                SpriteRenderer.transform.position = (v1 + v2) / 2;
                
                SpriteRenderer.size = new Vector2(distance, GetCurrentWidth());
                SpriteRenderer.sprite = GetCurrentSprite();
            }

            void AlineCollider()
            {
                BoxCollider2D.size = SpriteRenderer.size;
            }
        }
    }
}