﻿using System;
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
        [Header("Units Settings")] 
        [SerializeField] [Min(0)] private int initialHp;
        [SerializeField] private UnitType unitType;
        [SerializeField] [Min(0)] private int initialArmor;
        [SerializeField] [Min(0)] private int meleeDamage;
        [SerializeField] [Min(0)] private int visibility;
        [SerializeField] [Min(0)] private int cost;
        
        [field: SerializeField] public bool AttackLocked { get; set; }
        [SerializeField] private UnitSize unitSize;
        [SerializeField] private Passability passability;
        [SerializeField] private CommandPriorityData priorityData;

        [Header("Action Points Settings")]
        [SerializeField] [Min(0)] protected int initialActionPoints;
        [SerializeField] private ActionPointsModifier attackPointsModifier;
        [SerializeField] private ActionPointsModifier movePointsModifier;

        private UnitMovementLogic movementLogic;

        [Header("DEBUG")] 
        [SerializeField] private bool debugMode;
        [SerializeField, ReadOnlyInspector] private Node node;
        [SerializeField, ReadOnlyInspector] private UnitDirection unitDirection;
        [SerializeField, ReadOnlyInspector] private int currentHp;
        [SerializeField, ReadOnlyInspector] private int currentArmor;
        [SerializeField, ReadOnlyInspector] private int _currentActionPoints;

        protected int currentActionPoints
        {
            get => _currentActionPoints;
            set
            {
                if(value < 0) Debug.LogError("Action Points can't be less than zero!");
                var previousValue = _currentActionPoints;
                _currentActionPoints = value;
                ActionPointChanged.Invoke(previousValue, value);
            }
        }


        [field: Header("Events")]
        [field: SerializeField]
        public UnityEvent<UnitSize, UnitDirection> UnitDirectionChange { get; private set; }

        [field: SerializeField] public UnityEvent<int, int> HpChanged { get; private set; }
        [field: SerializeField] public UnityEvent<int, int> ArmorChanged { get; private set; }
        [field: SerializeField] public UnityEvent<Unit> Died { get; private set; }
        [field: SerializeField] public UnityEvent<int, int> ActionPointChanged { get; private set; }

        public int CurrentHp
        {
            get => currentHp;
            private set
            {
                var before = currentHp;
                currentHp = Math.Min(Math.Max(0, value), initialHp);
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

        public int Visibility => visibility;
        public int Cost => cost;
        public UnitSize Size => unitSize;
        public Passability Passability => passability;
        public Node Node => node;
        public CommandPriorityData CommandPriorityData => priorityData;
        public int CurrentActionPoints 
        {
            get => _currentActionPoints;
        }

        private void Awake()
        {
            currentHp = initialHp;
            currentArmor = initialArmor;
            currentActionPoints = initialActionPoints;
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
            currentActionPoints = movePointsModifier.Modify(currentActionPoints);
        }

        public bool CanMoveTo([NotNull] Node target)
        {
            return OwnerCondition()
                   && SizeCondition()
                   && LineCondition();

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

            bool OwnerCondition()
            {
                return target.Owner == null || target.Owner == Owner || target.Owner != Owner && target.AllIsFree;
            }
        }

        public void TakeDamage(Hit hit)
        {
            var blockedDamage = Math.Min(hit.Damage, CurrentArmor);
            if (blockedDamage != 0)
                CurrentArmor -= blockedDamage;

            var notBlockedDamage = hit.Damage - blockedDamage;
            if (notBlockedDamage != 0)
                CurrentHp -= notBlockedDamage;
        }

        public void Heal(int healAmount)
        {
            if (healAmount < 0)
                throw new ArgumentException($"{nameof(healAmount)} > 0 !");
            CurrentHp += healAmount;
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
            target.TakeDamage(new Hit(MeleeDamage));
            currentActionPoints = attackPointsModifier.Modify(currentActionPoints);
        }

        public bool CanAttack([NotNull] Unit unit)
        {
            var line = node.GetLine(unit.node);
            return !AttackLocked &&
                   unit.basePlayer != basePlayer
                   && line != null
                   && (int) Passability >= (int) line.LineType;
        }

        public bool CanAttack([NotNull] Edge edge)
        {
            return !AttackLocked &&
                   edge.LineType >= LineType.CountryRoad
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

        public void OnTurnEndTo()
        {
            currentArmor = initialArmor;
            currentActionPoints = initialActionPoints;
        }


        public bool TryGetNeighbour([NotNullWhen(true)] out Unit neighbour)
        {
            neighbour = null;
            if (Size == UnitSize.Large)
                return false;
            if (node.LeftUnit == this && node.RightUnit != null)
            {
                neighbour = node.RightUnit;
                return true;
            }

            if (node.RightUnit == this && node.LeftUnit != null)
            {
                neighbour = node.LeftUnit;
                return true;
            }

            return false;
        }

        public bool IsNeighbour(Unit unit)
        {
            return node.LeftUnit == this && node.RightUnit == unit
                   || node.RightUnit == this && node.LeftUnit == unit;
        }
    }
}