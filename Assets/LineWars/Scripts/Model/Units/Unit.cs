using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Extensions.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Model
{
    [RequireComponent(typeof(UnitMovementLogic))]
    public sealed class Unit : Owned, IUnit<Node, Edge, Unit, Owned, BasePlayer>
    {
        [Header("Units Settings")] 
        [SerializeField, ReadOnlyInspector] private int index;

        [SerializeField] private string unitName;

        [SerializeField, Min(0)] private int maxHp;
        [SerializeField, Min(0)] private int maxArmor;
        [SerializeField, Min(0)] private int visibility;

        [SerializeField] private UnitType unitType;
        [SerializeField] private UnitSize unitSize;
        [SerializeField] private LineType movementLineType;
        [SerializeField] private CommandPriorityData priorityData;

        [Header("Actions Settings")] 
        [SerializeField] [Min(0)] private int initialActionPoints;

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
        public event Action<IExecutorAction> CurrentActionCompleted;

        private UnitMovementLogic movementLogic;
        private Dictionary<CommandType, MonoUnitAction> runtimeActionsDictionary;
        public IEnumerable<MonoUnitAction> Actions => runtimeActionsDictionary.Values;
        private uint maxPossibleActionRadius;

        #region Properties
        public int Id => index;
        public string UnitName => unitName;
        public int InitialActionPoints => initialActionPoints;

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

        public int MaxHp => maxHp;
        public int MaxArmor => maxArmor;

        public int CurrentHp
        {
            get => currentHp;
            set
            {
                var before = currentHp;
                currentHp = Mathf.Min(Mathf.Max(0, value), maxHp);
                HpChanged.Invoke(before, currentHp);
                if (currentHp == 0)
                {
                    OnDied();
                    Died.Invoke(this);
                }
            }
        }

        public bool IsDied => CurrentHp <= 0;

        public int CurrentArmor
        {
            get => currentArmor;
            set
            {
                var before = currentArmor;
                currentArmor = Mathf.Max(0, value);
                ArmorChanged.Invoke(before, currentArmor);
            }
        }

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

        public int MaxActionPoints => initialActionPoints;

        #endregion

        private void Awake()
        {
            currentHp = maxHp;
            currentArmor = maxArmor;
            currentActionPoints = initialActionPoints;

            movementLogic = GetComponent<UnitMovementLogic>();

            InitialiseAllActions();

            void InitialiseAllActions()
            {
                var serializeActions = GetComponents<MonoUnitAction>()
                        .OrderByDescending(x => x.InitializePriority)
                        .ToArray();
                runtimeActionsDictionary = new Dictionary<CommandType, MonoUnitAction>(serializeActions.Length);
                foreach (var serializeAction in serializeActions)
                {
                    serializeAction.Initialize();
                    runtimeActionsDictionary.Add(serializeAction.GetMyCommandType(), serializeAction);
                    serializeAction.ActionCompleted += () =>
                    {
                        AnyActionCompleted?.Invoke();
                        CurrentActionCompleted?.Invoke(serializeAction);
                    };
                }
                
                maxPossibleActionRadius = Actions.Max(x => x.GetPossibleMaxRadius());
            }
        }

        public void Initialize(Node node, UnitDirection direction)
        {
            Node = node;
            UnitDirection = direction;
        }

        public T GetUnitAction<T>() where T : IUnitAction<Node, Edge, Unit, Owned, BasePlayer> 
            => Actions.OfType<T>().FirstOrDefault();

        public bool TryGetUnitAction<T>(out T action) where T : IUnitAction<Node, Edge, Unit, Owned, BasePlayer>
        {
            action = GetUnitAction<T>();
            return action != null;
        }

        public bool TryGetCommand(CommandType priorityType, ITarget target, out ICommand command)
        {
            if (runtimeActionsDictionary.TryGetValue(priorityType, out var value)
                && value is ITargetedAction targetedAction
                && targetedAction.IsMyTarget(target))
            {
                command = targetedAction.GenerateCommand(target);
                return true;
            }

            command = null;
            return false;
        }

        bool IReadOnlyExecutor.TryGetCommand(CommandType priorityType, IReadOnlyTarget target, out ICommand command)
        {
            return TryGetCommand(priorityType, (ITarget) target, out command);
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
            Destroy(gameObject);
        }

        protected override void OnReplenish()
        {
            CurrentActionPoints = initialActionPoints;

            foreach (var unitAction in Actions)
                unitAction.OnReplenish();
        }

        public IEnumerable<(IReadOnlyTarget, CommandType)> GetAllAvailableTargets()
        {
            return GetAllAvailableTargetsInRange(maxPossibleActionRadius + 1);
        }

        private IEnumerable<(IReadOnlyTarget, CommandType)> GetAllAvailableTargetsInRange(uint range)
        {
            foreach (var node in MonoGraph.Instance.GetNodesInRange(myNode, range))
            {
                yield return (node, UnitsController.Instance.GetCommandTypeBy(this, node));
                if (node.LeftUnit != null)
                    yield return (node.LeftUnit, UnitsController.Instance.GetCommandTypeBy(this, node.LeftUnit));
                if (node.RightUnit != null)
                    yield return (node.RightUnit, UnitsController.Instance.GetCommandTypeBy(this, node.RightUnit));

                foreach (var edge in node.Edges)
                {
                    yield return (edge, UnitsController.Instance.GetCommandTypeBy(this, edge));
                }
            }
        }
    }
}