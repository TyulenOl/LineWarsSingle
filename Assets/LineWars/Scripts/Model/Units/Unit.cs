using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LineWars.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Model
{
    [RequireComponent(typeof(UnitMovementLogic))]
    public sealed class Unit : Owned,
        IUnit<Node, Edge, Unit>, 
        IMonoExecutor,
        IMonoTarget
    {
        [Header("Units Settings")] 
        [SerializeField, ReadOnlyInspector] private int index;

        [SerializeField] private string unitName;
        [SerializeField][TextArea] private string unitDescription;
        
        [SerializeField, Min(0)] private int maxHp;
        [SerializeField, Min(0)] private int maxArmor;
        [SerializeField, Min(0)] private int visibility;
        [field: SerializeField] public Sprite Sprite { get; private set; }
        
        [SerializeField] private UnitType unitType;
        [SerializeField] private UnitSize unitSize;
        [SerializeField] private LineType movementLineType;
        [SerializeField] private CommandPriorityData priorityData;

        [Header("Sounds")] 
        [SerializeField] [Min(0)] private SFXList HpHealedSounds;
        [SerializeField] [Min(0)] private SFXList HpDamagedSounds;
        private IDJ dj;
        
        [Header("Actions Settings")] 
        [SerializeField] [Min(0)] private int maxActionPoints;

        [Header("DEBUG")] 
        [SerializeField, ReadOnlyInspector] private Node myNode;

        [SerializeField, ReadOnlyInspector] private UnitDirection unitDirection;
        [SerializeField, ReadOnlyInspector] private int currentHp;
        [SerializeField, ReadOnlyInspector] private int currentArmor;
        [SerializeField, ReadOnlyInspector] private int currentActionPoints;


        [field: Header("Events")]
        [field: SerializeField] public UnityEvent<UnitSize, UnitDirection> UnitDirectionChange { get; private set; }
        [field: SerializeField] public UnityEvent<int, int> HpChanged { get; private set; }
        [field: SerializeField] public UnityEvent<int, int> ArmorChanged { get; private set; }
        [field: SerializeField] public UnityEvent<Unit> Died { get; private set; }
        [field: SerializeField] public UnityEvent<int, int> ActionPointsChanged { get; private set; }

        public event Action AnyActionCompleted;

        private UnitMovementLogic movementLogic;
        
        
        private Dictionary<CommandType, IMonoUnitAction<UnitAction<Node, Edge, Unit>>> monoActionsDictionary;
        public IEnumerable<IMonoUnitAction<UnitAction<Node, Edge, Unit>>> MonoActions => monoActionsDictionary.Values;
        public uint MaxPossibleActionRadius { get; private set; }

        #region Properties
        public int Id => index;
        public string UnitName => unitName;
        
        public int MaxActionPoints
        {
            get => maxActionPoints;
            set
            {
                maxActionPoints = Mathf.Max(value, 0);
                CurrentActionPoints = Mathf.Min(currentActionPoints, maxActionPoints);
            }
        }

        public int CurrentActionPoints
        {
            get => currentActionPoints;
            set
            {
                var previousValue = currentActionPoints;
                currentActionPoints = Mathf.Max(0, value);
                ActionPointsChanged.Invoke(previousValue, currentActionPoints);
            }
        }

        public int MaxHp
        {
            get => maxHp;
            set
            {
                maxHp = Mathf.Max(value, 0);
                CurrentHp = Mathf.Min(currentHp, maxHp);
            }
        }

        public int CurrentHp
        {
            get => currentHp;
            set
            {
                var before = currentHp;
                currentHp = Mathf.Min(Mathf.Max(0, value), maxHp);
                if(before == currentHp) return;
                HpChanged.Invoke(before, currentHp);
                SfxManager.Instance.Play(before < currentHp ? dj.GetSound(HpHealedSounds) : dj.GetSound(HpDamagedSounds));

                if (currentHp == 0)
                {
                    OnDied();
                    Died.Invoke(this);
                }
            }
        }

        public int MaxArmor
        {
            get => maxArmor;
            set => maxArmor = Mathf.Max(value, 0);
        }

        public int CurrentArmor
        {
            get => currentArmor;
            set
            {
                var before = currentArmor;
                currentArmor = Mathf.Max(0, value);
                if(before == currentArmor) return;
                ArmorChanged.Invoke(before, currentArmor);
            }
        }

        public string UnitDescription => unitDescription;
        public UnitType Type => unitType;

        public UnitDirection UnitDirection
        {
            get => unitDirection;
            set
            {
                unitDirection = value;
                UnitDirectionChange?.Invoke(Size, unitDirection);
            }
        }

        public int Visibility
        {
            get => visibility;
            set => visibility = value;
        }

        public UnitSize Size => unitSize;
        public LineType MovementLineType => movementLineType;

        public Node Node
        {
            get => myNode;
            set
            {
                if (value == null)
                    throw new ArgumentException();
                myNode = value;
            }
        }

        public CommandPriorityData CommandPriorityData => priorityData;
        public bool CanDoAnyAction => currentActionPoints > 0;

        public UnitMovementLogic MovementLogic => movementLogic;

        public bool IsDied => CurrentHp <= 0;

        #endregion
        
        private void Awake()
        {
            dj = new RandomDJ(0.5f);
            
            currentHp = maxHp;
            currentArmor = 0;
            currentActionPoints = maxActionPoints;

            movementLogic = GetComponent<UnitMovementLogic>();

            InitialiseAllActions();
            index = SingleGame.Instance.AllUnits.Add(this);
            void InitialiseAllActions()
            {
                var serializeActions = gameObject.GetComponents<Component>()
                    .OfType<IMonoUnitAction<UnitAction<Node, Edge, Unit>>>()
                    .OrderByDescending(x => x.Priority)
                    .ToArray();

                monoActionsDictionary = new Dictionary<CommandType, IMonoUnitAction<UnitAction<Node, Edge, Unit>>>(serializeActions.Length);
                foreach (var serializeAction in serializeActions)
                {
                    serializeAction.Initialize();
                    monoActionsDictionary.Add(serializeAction.CommandType, serializeAction);
                    serializeAction.ActionCompleted += () =>
                    {
                        AnyActionCompleted?.Invoke();
                    };
                }

                MaxPossibleActionRadius = MonoActions.Max(x => x.GetPossibleMaxRadius());
            }
        }

        public void Initialize(Node node, UnitDirection direction)
        {
            Node = node;
            UnitDirection = direction;
        }

        public IEnumerable<IUnitAction<Node, Edge, Unit>> Actions => MonoActions;
        IEnumerable<IExecutorAction> IExecutorActionSource.Actions => Actions;

        public T GetUnitAction<T>() where T : IUnitAction<Node, Edge, Unit>
            => MonoActions.OfType<T>().FirstOrDefault();

        public bool TryGetUnitAction<T>(out T action) where T : IUnitAction<Node, Edge, Unit>
        {
            action = GetUnitAction<T>();
            return action != null;
        }

        private void OnDied()
        {
            if (unitSize == UnitSize.Large)
            {
                myNode.LeftUnit = null;
                myNode.RightUnit = null;
            }
            else if (UnitDirection == UnitDirection.Left)
            {
                myNode.LeftUnit = null;
            }
            else
            {
                myNode.RightUnit = null;
            }

            Owner.RemoveOwned(this);
            SingleGame.Instance.AllUnits.Remove(this);
            Destroy(gameObject);
        }

        protected override void OnReplenish()
        {
            CurrentActionPoints = maxActionPoints;

            foreach (var unitAction in MonoActions)
                unitAction.OnReplenish();
        }

        public T Accept<T>(IMonoExecutorVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}