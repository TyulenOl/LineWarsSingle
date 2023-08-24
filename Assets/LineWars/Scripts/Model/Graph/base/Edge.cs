using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Extensions.Attributes;
using LineWars.Interface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace LineWars.Model
{
    [Serializable]
    public class LineTypeCharacteristics
    {
        [SerializeField] private LineType lineType;
        [SerializeField, Min(0)] private int maxHp;
        [SerializeField] private Sprite sprite;

        public LineType LineType => lineType;
        public int MaxHp => maxHp;
        public Sprite Sprite => sprite;

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
        [Header("Graph Settings")] 
        [SerializeField] private int index;

        [SerializeField] [ReadOnlyInspector] private Node firstNode;
        [SerializeField] [ReadOnlyInspector] private Node secondNode;

        [Header("Line Settings")]
        [SerializeField] private LineType lineType;

        [SerializeField, NamedArray("lineType")] private List<LineTypeCharacteristics> lineTypeCharacteristics;

        [Header("Commands Settings")]
        [SerializeField] private CommandPriorityData priorityData;

        [Header("DEBUG")] 
        [SerializeField] [ReadOnlyInspector] private int hp;
        
        [field: Header("Events")]
        [field: SerializeField] public UnityEvent<int, int> HpChanged { get; private set; }
        [field: SerializeField] public UnityEvent<LineType, LineType> LineTypeChanged { get; private set; }

        private Dictionary<LineType, LineTypeCharacteristics> lineTypeCharacteristicsMap;
        [SerializeField, HideInInspector] private SpriteRenderer spriteRenderer;
        [SerializeField, HideInInspector] private BoxCollider2D boxCollider2D;

        public int Index
        {
            get => index;
            set => index = value;
        }

        public int MaxHp => lineTypeCharacteristicsMap.ContainsKey(LineType)
            ? lineTypeCharacteristicsMap[LineType].MaxHp
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
            }
        }

        public CommandPriorityData CommandPriorityData => priorityData;
        
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
            lineTypeCharacteristicsMap = new Dictionary<LineType, LineTypeCharacteristics>();

            for (int i = 0; i != lineTypeCharacteristics.Count; i++)
                lineTypeCharacteristicsMap.TryAdd(lineTypeCharacteristics[i].LineType, lineTypeCharacteristics[i]);

            UpdateTypes();
        }

        private void UpdateTypes()
        {
            foreach (var value in Enum.GetValues(typeof(LineType)).OfType<LineType>())
            {
                if (!lineTypeCharacteristicsMap.ContainsKey(value))
                    lineTypeCharacteristicsMap[value] = new LineTypeCharacteristics(value);
            }
        }

        public void LevelUp()
        {
            LineType = LineTypeHelper.Up(LineType);
        }
        
        public void ReDraw()
        {
            RedrawLine();
            AlineCollider();
        }

        private void RedrawLine()
        {
            var positionFirst = firstNode.Position;
            var positionSecond = secondNode.Position;
            var distance = Vector3.Distance(positionFirst, positionSecond);
            spriteRenderer.size = new Vector2(distance,spriteRenderer.size.y);
            var center = positionFirst;
            var newSecondNodePosition = positionSecond - center;
            var radian = Mathf.Atan2(newSecondNodePosition.y , newSecondNodePosition.x) * 180 / Math.PI;
            spriteRenderer.transform.rotation = Quaternion.Euler(0,0,(float)radian);
            spriteRenderer.transform.position = (positionFirst + positionSecond) / 2;

            spriteRenderer.sprite = lineTypeCharacteristicsMap.TryGetValue(lineType, out var ch)
                ? ch.Sprite
                : Resources.Load<Sprite>("Sprites/Road");
        }

        public void AlineCollider()
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