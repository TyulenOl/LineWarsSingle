﻿using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Extensions.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Model
{
    [Serializable]
    public class LineTypeCharacteristics
    {
        [SerializeField] private LineType lineType;
        [SerializeField, Min(0)] private int maxHp;
        [SerializeField] private Sprite sprite;
        [SerializeField, Min(0)] private float width = 5;

        public LineType LineType => lineType;
        public int MaxHp => maxHp;
        public Sprite Sprite => sprite;

        public float Width => width;

        public LineTypeCharacteristics(LineType type)
        {
            lineType = type;
            maxHp = 0;
        }
    }

    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Edge : MonoBehaviour, IAlive, ITarget, INumbered, ISerializationCallbackReceiver
    {
        [Header("Graph Settings")] [SerializeField]
        private int index;

        [SerializeField] private Node firstNode;
        [SerializeField] private Node secondNode;

        [Header("Line Settings")] [SerializeField]
        private LineType lineType;

        [SerializeField, NamedArray("lineType")]
        private List<LineTypeCharacteristics> lineTypeCharacteristics;

        [Header("Commands Settings")] [SerializeField]
        private CommandPriorityData priorityData;

        [Header("DEBUG")] [SerializeField] [ReadOnlyInspector]
        private int hp;

        [field: Header("Events")]
        [field: SerializeField]
        public UnityEvent<int, int> HpChanged { get; private set; }

        [field: SerializeField] public UnityEvent<LineType, LineType> LineTypeChanged { get; private set; }

        private Dictionary<LineType, LineTypeCharacteristics> lineMap;
        [SerializeField, HideInInspector] private SpriteRenderer spriteRenderer;
        [SerializeField, HideInInspector] private BoxCollider2D boxCollider2D;

        public SpriteRenderer SpriteRenderer => spriteRenderer;
        public BoxCollider2D BoxCollider2D => boxCollider2D;

        public int Index
        {
            get => index;
            set => index = value;
        }

        public int MaxHp => lineMap.ContainsKey(LineType)
            ? lineMap[LineType].MaxHp
            : 0;

        public Node FirstNode => firstNode;
        public Node SecondNode => secondNode;

        public int CurrentHp
        {
            get => hp;
            private set
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
            private set
            {
                var before = lineType;
                lineType = value;
                LineTypeChanged.Invoke(before, lineType);
                CurrentHp = MaxHp;
                RedrawLine();
            }
        }

        public CommandPriorityData CommandPriorityData => priorityData;

        private float CurrentWidth => lineMap.TryGetValue(lineType, out var ch) 
            ? ch.Width 
            : 0.1f;

        private Sprite CurrentSprite => lineMap.TryGetValue(lineType, out var ch)
            ? ch.Sprite
            : Resources.Load<Sprite>("Sprites/Road");


        protected void OnValidate()
        {
            hp = MaxHp;
            AssignFields();
        }


        public void Initialize(Node firstNode, Node secondNode)
        {
            this.firstNode = firstNode;
            this.secondNode = secondNode;
        }

        public void TakeDamage(Hit hit)
        {
            CurrentHp -= hit.Damage;
        }

        public Node GetOther(Node node)
        {
            if (FirstNode.Equals(node))
                return SecondNode;
            else
                return FirstNode;
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
        }

        private void UpdateTypes()
        {
            foreach (var value in Enum.GetValues(typeof(LineType)).OfType<LineType>())
            {
                if (!lineMap.ContainsKey(value))
                    lineMap[value] = new LineTypeCharacteristics(value);
            }
        }

        public void LevelUp()
        {
            LineType = LineTypeHelper.Up(LineType);
        }

        public void Redraw()
        {
            name = $"Edge{Index} from {(FirstNode ? FirstNode.name : "Null")} to {(SecondNode ? SecondNode.name : "None")}";
            RedrawLine();
            AlineCollider();
        }

        private void RedrawLine()
        {
            var v1 = firstNode?firstNode.Position: Vector2.zero;
            var v2 = secondNode?secondNode.Position: Vector2.right;
            var distance = Vector2.Distance(v1, v2);
            var center = v1;
            var newSecondNodePosition = v2 - center;
            var radian = Mathf.Atan2(newSecondNodePosition.y, newSecondNodePosition.x) * 180 / Mathf.PI;
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, radian);
            spriteRenderer.transform.position = (v1 + v2) / 2;

            spriteRenderer.size = new Vector2(distance, CurrentWidth);
            spriteRenderer.sprite = CurrentSprite;
        }


        private void AlineCollider()
        {
            boxCollider2D.size = spriteRenderer.size;
        }

        private void AssignFields()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();
            if (boxCollider2D == null)
                boxCollider2D = GetComponent<BoxCollider2D>();
        }
    }
}