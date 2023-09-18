using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LineWars.Extensions.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Model
{
    [RequireComponent(typeof(UnitMovementLogic))]
    public sealed partial class ComponentUnit: Owned, ITarget, IExecutor, IAlive
    {
        [Header("Units Settings")] 
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
        [SerializeField] private List<BaseUnitActionData> serializeActions;
        
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
        [field: SerializeField] public UnityEvent<ComponentUnit> Died { get; private set; }
        [field: SerializeField] public UnityEvent<int, int> ActionPointsChanged { get; private set; }
        [field: SerializeField] public UnityEvent AnyActionCompleted { get; private set; }
        
        private UnitMovementLogic movementLogic;
        private List<UnitAction> runtimeActions;
        private uint maxPossibleActionRadius;

        #region Properties
        public string UnitName => unitName;
        public int CurrentActionPoints
        {
            get => currentActionPoints;
            private set
            {
                var previousValue = currentActionPoints;
                currentActionPoints = Mathf.Max(0, value);
                ActionPointsChanged.Invoke(previousValue, currentActionPoints);
            }
        }

        public int MaxHp => maxHp;
        public int Armor => maxArmor;

        public int CurrentHp
        {
            get => currentHp;
            private set
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
            private set
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
            private set
            {
                unitDirection = value;
                UnitDirectionChange?.Invoke(Size, unitDirection);
            }
        }
        
        public int Visibility => visibility;
        public UnitSize Size => unitSize;
        public LineType MovementLineType => movementLineType;
        public Node Node => myNode;
        public CommandPriorityData CommandPriorityData => priorityData;
        public bool CanDoAnyAction => currentActionPoints > 0;
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
                runtimeActions = new List<UnitAction>(serializeActions.Count);
                foreach (var serializeAction in serializeActions)
                {
                    ExecutorAction runtimeAction = serializeAction.GetAction(this);
                    runtimeActions.Add((UnitAction)runtimeAction);

                    runtimeAction.ActionCompleted += () => AnyActionCompleted?.Invoke();
                }
                maxPossibleActionRadius = runtimeActions.Max(x => x.GetPossibleMaxRadius());
            }
        }
        
        public void Initialize(Node node, UnitDirection direction)
         {
             myNode = node;
             UnitDirection = direction;
         }

        public bool CanMoveOnLineWithType(LineType lineType) => lineType >= MovementLineType;

        public T GetExecutorAction<T>() where T : ExecutorAction => runtimeActions.OfType<T>().FirstOrDefault();
        public bool TryGetExecutorAction<T>(out T action) where T : ExecutorAction
        {
            action = GetExecutorAction<T>();
            return action != null;
        }
        
        public bool TryGetNeighbour([NotNullWhen(true)] out ComponentUnit neighbour)
        {
            neighbour = null;
            if (Size == UnitSize.Large)
                return false;
            if (myNode.LeftUnit == this && myNode.RightUnit != null)
            {
                neighbour = myNode.RightUnit;
                return true;
            }
            
            if (myNode.RightUnit == this && myNode.LeftUnit != null)
            {
                neighbour = myNode.LeftUnit;
                return true;
            }
            
            return false;
        }
        
        public bool IsNeighbour(ComponentUnit unit)
        {
            return myNode.LeftUnit == this && myNode.RightUnit == unit
                   || myNode.RightUnit == this && myNode.LeftUnit == unit;
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

            foreach (var unitAction in runtimeActions)
                unitAction.OnReplenish();
        }
        public void TakeDamage(Hit hit)
        {
            var blockedDamage = Math.Min(hit.Damage, CurrentArmor);

            var notBlockedDamage = hit.IsPenetrating ? hit.Damage : hit.Damage - blockedDamage;
            if (notBlockedDamage != 0)
                CurrentHp -= notBlockedDamage;

            if (!IsDied && hit is {IsRangeAttack: false})
                UnitsController.ExecuteCommand(new ContrAttackCommand(this, hit.Source), false);
        }
        
        public void HealMe(int healAmount)
        {
             if (healAmount < 0)
                 throw new ArgumentException($"{nameof(healAmount)} > 0 !");
             CurrentHp += healAmount;
        }

        public IEnumerable<(ITarget, CommandType)> GetAllAvailableTargets()
        {
            return GetAllAvailableTargetsInRange(maxPossibleActionRadius);
        }

        private IEnumerable<(ITarget, CommandType)> GetAllAvailableTargetsInRange(uint range)
         {
             var visibilityEdges = new HashSet<Edge>();
             foreach (var e in Graph.GetNodesInRange(myNode, range))
             {
                 foreach (var target in e.GetTargetsWithMe())
                     yield return (target, UnitsController.Instance.GetCommandTypeBy(this, target));
                 
                 foreach (var edge in myNode.Edges)
                 {
                     if (visibilityEdges.Contains(edge)) continue;
                     visibilityEdges.Add(edge);
                     yield return (edge, UnitsController.Instance.GetCommandTypeBy(this, edge));
                 }
             }
         }
    }
}