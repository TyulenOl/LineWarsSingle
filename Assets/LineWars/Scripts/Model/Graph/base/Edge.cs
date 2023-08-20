using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Extensions.Attributes;
using LineWars.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Model
{
    [Serializable]
    public class LineTypeCharacteristics
    {
        [field: SerializeField] public LineType LineType { get; set; }
        [field: SerializeField, Min(0)] public int MaxHp { get; set; }

        public LineTypeCharacteristics(LineType type)
        {
            LineType = type;
            MaxHp = 0;
        }
    }

    public class Edge : MonoBehaviour, IAlive, ITarget, INumbered, ISerializationCallbackReceiver
    {
        [Header("Graph Settings")] [SerializeField]
        private int index;

        [SerializeField] [ReadOnlyInspector] private Node firstNode;
        [SerializeField] [ReadOnlyInspector] private Node secondNode;

        [Header("Line Settings")] [SerializeField]
        private LineType lineType;

        [SerializeField] private List<LineTypeCharacteristics> lineTypeCharacteristics;

        [Header("Commands Settings")] [SerializeField]
        private CommandPriorityData priorityData;

        [Header("DEBUG")] 
        [SerializeField] [ReadOnlyInspector] private int hp;

        [SerializeField] [HideInInspector] private LineDrawer drawer;

        [field: Header("Events")]
        [field: SerializeField]
        public UnityEvent<int, int> HpChanged { get; private set; }

        [field: SerializeField] public UnityEvent<LineType, LineType> LineTypeChanged { get; private set; }
        [field: SerializeField] public UnityEvent<Unit> Died { get; private set; }

        private Dictionary<LineType, LineTypeCharacteristics> lineTypeCharacteristicsMap;

        public int Index
        {
            get => index;
            set => index = value;
        }

        public int MaxHp => lineTypeCharacteristicsMap[LineType].MaxHp;
        
        public Node FirstNode => firstNode;
        public Node SecondNode => secondNode;
        public LineDrawer Drawer => drawer;

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

        public bool IsDied => CurrentHp <= 0;

        public LineType LineType
        {
            get => lineType;
            private set
            {
                var before = lineType;
                lineType = value;
                LineTypeChanged.Invoke(before, lineType);
                hp = MaxHp;
            }
        }

        public CommandPriorityData CommandPriorityData => priorityData;

        protected void OnValidate()
        {
            hp = MaxHp;
        }

        public void Initialize(Node firstNode, Node secondNode, LineType lineType = LineType.InfantryRoad)
        {
            this.firstNode = firstNode;
            this.secondNode = secondNode;
            drawer = GetComponent<LineDrawer>();
            drawer.Initialise(firstNode.transform, secondNode.transform);
            this.lineType = lineType;
        }

        public void TakeDamage(Hit hit)
        {
            CurrentHp -= hit.Damage;
        }
        
        public void ReDraw() => drawer.DrawLine();

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
                lineTypeCharacteristicsMap.Add(lineTypeCharacteristics[i].LineType, lineTypeCharacteristics[i]);

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
    }
}