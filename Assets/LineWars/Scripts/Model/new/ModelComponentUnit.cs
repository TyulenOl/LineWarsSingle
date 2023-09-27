using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine.Events;

namespace LineWars.Model
{
    public sealed partial class ModelComponentUnit: ModelOwned, ITarget, IExecutor, IAlive
    {
        private int Index { get; }
        public string UnitName { get; }
        public int MaxHp { get; }
        public int MaxArmor { get; }
        public UnitType Type { get; }
        public UnitSize Size { get; }
        public LineType MovementLineType { get; }
        public int Visibility { get; }
        public CommandPriorityData CommandPriorityData { get; }
        public int InitialActionPoints { get; }
        public ModelNode Node { get; private set; }
        
        private UnitDirection unitDirection;
        public UnitDirection UnitDirection
        {
            get => unitDirection;
            private set
            {
                unitDirection = value;
                UnitDirectionChange?.Invoke(Size, unitDirection);
            }
        }
        
        private int currentHp;
        public int CurrentHp
        {
            get => currentHp;
            private set
            {
                var before = currentHp;
                currentHp = Math.Min(Math.Max(0, value), MaxHp);
                HpChanged?.Invoke(before, currentHp);
                if (currentHp == 0)
                {
                    OnDied();
                    Died?.Invoke(this);
                }
            }
        }
        
        private int currentArmor;
        public int CurrentArmor
        {
            get => currentArmor;
            private set
            {
                var before = currentArmor;
                currentArmor = Math.Max(0, value);
                ArmorChanged?.Invoke(before, currentArmor);
            }
        }
        private int currentActionPoints;
        public int CurrentActionPoints
        {
            get => currentActionPoints;
            private set
            {
                var previousValue = currentActionPoints;
                currentActionPoints = Math.Max(0, value);
                ActionPointsChanged?.Invoke(previousValue, currentActionPoints);
            }
        }

        
        private readonly Dictionary<CommandType, UnitAction> runtimeActionsDictionary;
        private IEnumerable<UnitAction> RuntimeActions => runtimeActionsDictionary.Values;
        private uint maxPossibleActionRadius;
        private UnityEvent anyActionCompleted;
        
        public bool IsDied => CurrentHp <= 0;
        
        public bool CanDoAnyAction => currentActionPoints > 0;

        public event Action<UnitSize, UnitDirection> UnitDirectionChange;
        public event Action<int, int> HpChanged;
        public event Action<int, int> ArmorChanged;
        public event Action<ModelComponentUnit> Died;
        public event Action<int, int> ActionPointsChanged;
        public event Action AnyActionCompleted;
        public event Action<UnitAction> CurrentActionCompleted;

        public ModelComponentUnit(
            [NotNull] ModelBasePlayer basePlayer,
            int index,
            string unitName,
            int maxHp,
            int maxArmor,
            UnitDirection unitDirection,
            UnitType type,
            UnitSize size,
            LineType movementLineType,
            int visibility,
            [NotNull] CommandPriorityData commandPriorityData,
            int initialActionPoints,
            [NotNull] ModelNode node,
            [NotNull] IEnumerable<UnitAction> actions) : base(basePlayer)
        {
            if (basePlayer == null) throw new ArgumentNullException(nameof(basePlayer));
            if (commandPriorityData == null) throw new ArgumentNullException(nameof(commandPriorityData));
            if (node == null)  throw new ArgumentNullException(nameof(node));
            if (actions == null) throw new ArgumentNullException(nameof(actions));
            
            Index = index;
            UnitName = unitName;
            MaxHp = maxHp;
            MaxArmor = maxArmor;
            UnitDirection = unitDirection;
            Type = type;
            Size = size;
            MovementLineType = movementLineType;
            Visibility = visibility;
            CommandPriorityData = commandPriorityData;
            InitialActionPoints = initialActionPoints;
            Node = node;

            runtimeActionsDictionary = actions
                .ToDictionary(action => action.GetMyCommandType());
        }
        
        public bool CanMoveOnLineWithType(LineType lineType) => lineType >= MovementLineType;

        public T GetExecutorAction<T>() where T : ExecutorAction => RuntimeActions.OfType<T>().FirstOrDefault();
        public bool TryGetExecutorAction<T>(out T action) where T : ExecutorAction
        {
            action = GetExecutorAction<T>();
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

        public bool TryGetNeighbour([NotNullWhen(true)] out ModelComponentUnit neighbour)
        {
            neighbour = null;
            if (Size == UnitSize.Large)
                return false;
            if (Node.LeftUnit == this && Node.RightUnit != null)
            {
                neighbour = Node.RightUnit;
                return true;
            }
            
            if (Node.RightUnit == this && Node.LeftUnit != null)
            {
                neighbour = Node.LeftUnit;
                return true;
            }
            
            return false;
        }
        
        public bool IsNeighbour(ModelComponentUnit unit)
        {
            return Node.LeftUnit == this && Node.RightUnit == unit
                   || Node.RightUnit == this && Node.LeftUnit == unit;
        }
        
        
        private void OnDied()
        {
            if (Size == UnitSize.Large)
            {
                Node.LeftUnit = null;
                Node.RightUnit = null;
            }
            else if (UnitDirection == UnitDirection.Left)
            {
                Node.LeftUnit = null;
            }
            else
            {
                Node.RightUnit = null;
            }

            Owner.RemoveOwned(this);
        }
        
        protected override void OnReplenish()
        {
            CurrentActionPoints = InitialActionPoints;

            foreach (var unitAction in RuntimeActions)
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
            return GetAllAvailableTargetsInRange(maxPossibleActionRadius + 1);
        }

        private IEnumerable<(ITarget, CommandType)> GetAllAvailableTargetsInRange(uint range)
         {
             var visibilityEdges = new HashSet<ModelEdge>();
             foreach (var e in Graph.GetNodesInRange(Node, range))
             {
                 foreach (var target in e.GetTargetsWithMe())
                     yield return (target, UnitsController.Instance.GetCommandTypeBy(this, target));
                 
                 foreach (var edge in Node.Edges)
                 {
                     if (visibilityEdges.Contains(edge)) continue;
                     visibilityEdges.Add(edge);
                     yield return (edge, UnitsController.Instance.GetCommandTypeBy(this, edge));
                 }
             }
         }
    }
}