using System;
using System.Diagnostics.CodeAnalysis;
using LineWars.Controllers;
using LineWars.Extensions.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Model
{
    public class Unit : Owned,
        IAttackerVisitor,
        IAlive,
        IMovable,
        ITarget,
        IExecutor,
        ITurnEndAction
    {
        [Header("Units Settings")] [SerializeField] [Min(0)]
        private int initialHp;

        [SerializeField] [Min(0)] private int initialArmor;
        [SerializeField] [Min(0)] private int meleeDamage;
        [SerializeField] [Min(0)] private int initialSpeedPoints;
        [SerializeField] [Min(0)] private int visibility;
        [SerializeField] [Min(0)] private int cost;
        [SerializeField] private UnitSize unitSize;
        [SerializeField] private Passability passability;
        [SerializeField] private CommandPriorityData priorityData;

        private UnitMovementLogic movementLogic;

        [Header("DEBUG")] [SerializeField] private bool debugMode;
        [SerializeField, ReadOnlyInspector] private Node node;
        [SerializeField, ReadOnlyInspector] private UnitDirection unitDirection;
        [SerializeField, ReadOnlyInspector] private int currentHp;
        [SerializeField, ReadOnlyInspector] private int currentArmor;
        [SerializeField, ReadOnlyInspector] private int currentSpeedPoints;


        [field: Header("Events")]
        [field: SerializeField]
        public UnityEvent<UnitSize, UnitDirection> UnitDirectionChange { get; private set; }

        [field: SerializeField] public UnityEvent<int, int> HpChanged { get; private set; }
        [field: SerializeField] public UnityEvent<int, int> ArmorChanged { get; private set; }
        [field: SerializeField] public UnityEvent<Unit> Died { get; private set; }

        public int CurrentHp
        {
            get => currentHp;
            private set
            {
                var before = currentHp;
                currentHp = Math.Max(0, value);
                HpChanged.Invoke(before, currentHp);
                if (currentHp == 0)
                {
                    Died.Invoke(this);
                    OnDied();
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
                currentArmor = Math.Max(0, value);
                ArmorChanged.Invoke(before, currentArmor);
            }
        }

        public int MeleeDamage => meleeDamage;

        public int CurrentSpeedPoints
        {
            get => currentSpeedPoints;
            private set { currentSpeedPoints = Math.Max(0, value); }
        }

        private UnitDirection UnitDirection
        {
            get => unitDirection;
            set
            {
                unitDirection = value;
                UnitDirectionChange?.Invoke(Size, unitDirection);
            }
        }

        public int Visibility => visibility;
        public int Cost => cost;
        public UnitSize Size => unitSize;
        public Passability Passability => passability;
        public Node Node => node;
        public CommandPriorityData CommandPriorityData => priorityData;

        private void Awake()
        {
            currentHp = initialHp;
            currentSpeedPoints = initialSpeedPoints;
            currentArmor = initialArmor;

            movementLogic = GetComponent<UnitMovementLogic>();
        }

        protected void OnValidate()
        {
            if (GetComponent<UnitMovementLogic>() == null)
            {
                Debug.LogError($"у {name} не обнаружен компонент {nameof(UnitMovementLogic)}");
            }
        }

        private void OnEnable()
        {
            movementLogic.MovementIsOver += MovementLogicOnMovementIsOver;
        }

        private void OnDisable()
        {
            movementLogic.MovementIsOver -= MovementLogicOnMovementIsOver;
        }

        private void MovementLogicOnMovementIsOver(Transform arg2)
        {
            CurrentSpeedPoints--;

            node = arg2.GetComponent<Node>();
            if (Size == UnitSize.Large)
            {
                node.LeftUnit = this;
                node.RightUnit = this;
            }
            else if (node.LeftIsFree && (UnitDirection == UnitDirection.Left ||
                                         UnitDirection == UnitDirection.Right && !node.RightIsFree))
            {
                node.LeftUnit = this;
                UnitDirection = UnitDirection.Left;
            }
            else
            {
                node.RightUnit = this;
                UnitDirection = UnitDirection.Right;
            }

            if (this.Owner != node.Owner)
                Owned.Connect(Owner, node);
        }

        public void Initialize(Node node, UnitDirection direction)
        {
            this.node = node;
            UnitDirection = direction;
        }

        public void MoveTo([NotNull] Node target)
        {
            if (node.LeftUnit == this)
                node.LeftUnit = null;
            if (node.RightUnit == this)
                node.RightUnit = null;

            movementLogic.MoveTo(target.transform);
        }

        public bool CanMoveTo([NotNull] Node target)
        {
            return OwnerCondition() && SizeCondition() && LineCondition() && SpeedCondition();

            bool SizeCondition()
            {
                return Size == UnitSize.Little && target.AnyIsFree
                       || Size == UnitSize.Large && target.AllIsFree;
            }

            bool LineCondition()
            {
                var line = node.GetLine(target);
                return line != null
                       && line.LineType != LineType.Visibility
                       && line.LineType != LineType.Firing
                       && (int) Passability >= (int) line.LineType;
            }

            bool SpeedCondition()
            {
                return CurrentSpeedPoints > 0 || debugMode;
            }

            bool OwnerCondition()
            {
                return target.Owner == null || target.Owner == Owner || target.Owner != Owner && target.AllIsFree;
            }
        }

        public void DealDamage(Hit hit)
        {
            var blockedDamage = Math.Min(hit.Damage, CurrentArmor);
            if (blockedDamage != 0)
                CurrentArmor -= blockedDamage;

            var notBlockedDamage = hit.Damage - blockedDamage;
            if (notBlockedDamage != 0)
                CurrentHp -= notBlockedDamage;
        }

        public void Attack([NotNull] Edge edge)
        {
            _Attack(edge);
        }

        public void Attack([NotNull] Unit enemy)
        {
            var enemyNode = enemy.node;
            _Attack(enemy);

            if (enemy.IsDied && enemyNode.AllIsFree)
                UnitsController.ExecuteCommand(new MoveCommand(this, node, enemyNode));
        }

        private void _Attack(IAlive target)
        {
            target.DealDamage(new Hit(MeleeDamage));
        }

        public bool CanAttack([NotNull] Unit unit)
        {
            var line = node.GetLine(unit.node);
            return unit.basePlayer != basePlayer
                   && line != null
                   && (int) Passability >= (int) line.LineType;
        }

        public bool CanAttack([NotNull] Edge edge)
        {
            return edge.LineType >= LineType.CountryRoad
                   && node.ContainsEdge(edge);
        }

        protected virtual void OnDied()
        {
            if (unitSize == UnitSize.Large)
            {
                node.LeftUnit = null;
                node.RightUnit = null;
            }
            else if (UnitDirection == UnitDirection.Left)
            {
                node.LeftUnit = null;
            }
            else
            {
                node.RightUnit = null;
            }

            Destroy(gameObject);
        }

        public void OnTurnEnd()
        {
            CurrentArmor = initialArmor;
            CurrentSpeedPoints = initialSpeedPoints;
        }
    }
}