using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LineWars.Extensions.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Model
{
    public sealed class CombinedUnit: Owned, IAlive
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
        [SerializeField] private List<UnitAction> myActions;
        
        [Header("DEBUG")]
        [SerializeField, ReadOnlyInspector] private Node myNode;
        [SerializeField, ReadOnlyInspector] private UnitDirection unitDirection;
        [SerializeField, ReadOnlyInspector] private int currentHp;
        [SerializeField, ReadOnlyInspector] private int currentArmor;
        [SerializeField, ReadOnlyInspector] private int currentActionPoints;
        [SerializeField, ReadOnlyInspector] private bool isBlocked;
        
        
        [field: Header("Events")]
        [field: SerializeField] public UnityEvent<UnitSize, UnitDirection> UnitDirectionChange { get; private set; }
        [field: SerializeField] public UnityEvent<int, int> HpChanged { get; private set; }
        [field: SerializeField] public UnityEvent<int, int> ArmorChanged { get; private set; }
        [field: SerializeField] public UnityEvent<CombinedUnit> Died { get; private set; }
        [field: SerializeField] public UnityEvent<int, int> ActionPointsChanged { get; private set; }
        [field: SerializeField] public UnityEvent<bool, bool> CanBlockChanged { get; private set; }
        [field: SerializeField] public UnityEvent ActionCompleted { get; private set; }

        #region Properties
        public string UnitName => unitName;
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

        private UnitDirection UnitDirection
        {
            get => unitDirection;
            set
            {
                unitDirection = value;
                UnitDirectionChange?.Invoke(Size, unitDirection);
            }
        }
        
        public bool IsBlocked
        {
            get => isBlocked;
            private set
            {
                var before = isBlocked;
                isBlocked = value;
                if (before != isBlocked)
                    CanBlockChanged.Invoke(before, isBlocked);
            }
        }

        public int Visibility => visibility;
        public UnitSize Size => unitSize;
        public LineType MovementLineType => movementLineType;
        public Node Node => myNode;
        public CommandPriorityData CommandPriorityData => priorityData;
        #endregion

        private void Awake()
        {
            currentHp = maxHp;
            currentArmor = maxArmor;
            currentActionPoints = initialActionPoints;
            
            InitialiseAllActions();
            
            void InitialiseAllActions()
            {
                var myActionsCopy = myActions.ToArray();
                myActions.Clear();
                foreach (var action in myActionsCopy)
                {
                    var copy = Instantiate(action);
                    copy.Initialize(this);
                    myActions.Add(copy);
                }
            }
        }

        public bool CanDoAnyAction() => currentActionPoints > 0;
        public bool CanMoveOnLineWithType(LineType lineType) => lineType >= MovementLineType;

        public T GetUnitAction<T>() where T : UnitAction => myActions.OfType<T>().FirstOrDefault();
        public bool TryGetUnitAction<T>(out T action) where T : UnitAction
        {
            action = GetUnitAction<T>();
            return action != null;
        }
        
        public bool TryGetNeighbour([NotNullWhen(true)] out CombinedUnit neighbour)
        {
            throw new NotImplementedException();
            // neighbour = null;
            // if (Size == UnitSize.Large)
            //     return false;
            // if (myNode.LeftUnit == this && myNode.RightUnit != null)
            // {
            //     neighbour = myNode.RightUnit;
            //     return true;
            // }
            //
            // if (myNode.RightUnit == this && myNode.LeftUnit != null)
            // {
            //     neighbour = myNode.LeftUnit;
            //     return true;
            // }
            //
            // return false;
        }
        
        public bool IsNeighbour(Unit unit)
        {
            throw new NotImplementedException();
            // return myNode.LeftUnit == this && myNode.RightUnit == unit
            //        || myNode.RightUnit == this && myNode.LeftUnit == unit;
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
            IsBlocked = false;

            foreach (var unitAction in myActions)
                unitAction.OnReplenish();
        }

        public void TakeDamage(Hit hit)
        {
            
        }
    }
}