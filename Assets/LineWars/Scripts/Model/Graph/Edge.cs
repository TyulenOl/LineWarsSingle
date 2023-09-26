using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using LineWars.Extensions.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Model
{
    [RequireComponent(typeof(Selectable2D))]
    public class Edge : MonoBehaviour, ITarget, INumbered, ISerializationCallbackReceiver
    {
        public ModelEdge Model { get; private set; }

        [Header("Graph Settings")]
        [SerializeField] private int index;

        [SerializeField] private Node firstNode;
        [SerializeField] private Node secondNode;

        [Header("Line Settings")] 
        [SerializeField] private LineType lineType;

        [SerializeField] private SerializedDictionary<LineType, LineTypeCharacteristics> lineMap;

        [Header("Commands Settings")] 
        [SerializeField] private CommandPriorityData priorityData;
        
        [Header("References")]
        [SerializeField] private BoxCollider2D edgeCollider;
        [SerializeField] private SpriteRenderer edgeSpriteRenderer;
        
        [field: Header("Events")]
        [field: SerializeField] public UnityEvent<int, int> HpChanged { get; private set; }
        [field: SerializeField] public UnityEvent<LineType, LineType> LineTypeChanged { get; private set; }
        
        public SpriteRenderer SpriteRenderer => edgeSpriteRenderer;
        public BoxCollider2D BoxCollider2D => edgeCollider;

        public int Index => Model.Index;
        public Node FirstNode => firstNode;
        public Node SecondNode => secondNode;

        public CommandPriorityData CommandPriorityData => priorityData;

        private float CurrentWidth => lineMap.TryGetValue(lineType, out var ch) 
            ? ch.Width 
            : 0.1f;

        private Sprite CurrentSprite => lineMap.TryGetValue(lineType, out var ch)
            ? ch.Sprite
            : Resources.Load<Sprite>("Sprites/Road");


        private void Awake()
        {
            Model = GenerateModel();
            if (Model == null)
                Debug.LogError("Model in Edge is null!");
        }

        public void Initialize(int index, Node firstNode, Node secondNode)
        {
            this.index = index;
            this.firstNode = firstNode;
            this.secondNode = secondNode;
        }

        public Node GetOther(Node node)
        {
            if (FirstNode.Equals(node))
                return SecondNode;
            else
                return FirstNode;
        }

        public void LevelUp() => Model.LevelUp();

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
            edgeSpriteRenderer.transform.rotation = Quaternion.Euler(0, 0, radian);
            edgeSpriteRenderer.transform.position = (v1 + v2) / 2;

            edgeSpriteRenderer.size = new Vector2(distance, CurrentWidth);
            edgeSpriteRenderer.sprite = CurrentSprite;
        }


        private void AlineCollider()
        {
            edgeCollider.size = edgeSpriteRenderer.size;
        }

        public void OnBeforeSerialize() {}

        public void OnAfterDeserialize()
        {
            Model = GenerateModel();
        }

        public ModelEdge GenerateModel()
        {
            try
            {
                var model = new ModelEdge(index,transform.position, lineType, firstNode.Model, secondNode.Model, priorityData, lineMap);
                model.HpChanged += (before, after) => HpChanged.Invoke(before, after);
                model.LineTypeChanged += (before, after) => LineTypeChanged.Invoke(before, after);
                return model;
            }
            catch (Exception _)
            {
                return null;
            }
        }
    }
    
    [Serializable]
    public class LineTypeCharacteristics
    {
        [SerializeField, Min(0)] private int maxHp;
        [SerializeField] private Sprite sprite;
        [SerializeField, Min(0)] private float width = 5;
        
        public int MaxHp => maxHp;
        public Sprite Sprite => sprite;

        public float Width => width;

        public LineTypeCharacteristics(LineType type)
        {
            maxHp = 0;
        }
    }
}