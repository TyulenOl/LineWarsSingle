using System;
using System.Diagnostics.CodeAnalysis;
using LineWars.Controllers;
using LineWars.Extensions.Attributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace LineWars.Model
{
    [RequireComponent(typeof(UnitMovementLogic))]
    public class Unit : Owned,
        IAttackerVisitor,
        IAlive,
        IMovable,
        ITarget,
        IExecutor
    {
        [Header("Units Settings")] 
        [SerializeField, Min(0)] private int maxHp;

        [SerializeField, Min(0)] private int initialArmor;
        [SerializeField, Min(0)] private int meleeDamage;
        [SerializeField, Min(0)] private int visibility;
        [SerializeField, Min(0)] private int cost;
        [field: SerializeField] public bool AttackLocked { get; private set; }

        [SerializeField] private UnitType unitType;
        [SerializeField] private UnitSize unitSize;
        [SerializeField] private Passability passability;
        [SerializeField] private CommandPriorityData priorityData;

        [Header("Action Points Settings")] 
        [SerializeField] [Min(0)] protected int initialActionPoints;

        [SerializeField] protected IntModifier attackPointsModifier;
        [SerializeField] protected IntModifier movePointsModifier;
        [SerializeField] protected IntModifier blockPointsModifier;

        [Header("Other")] 
        [SerializeField] private IntModifier contrAttackDamageModifier;

        private UnitMovementLogic movementLogic;
        private IUnitBlockerSelector blockerSelector;

        [Header("DEBUG")] [SerializeField, ReadOnlyInspector]
        private Node node;

        [SerializeField, ReadOnlyInspector] private UnitDirection unitDirection;
        [SerializeField, ReadOnlyInspector] private int currentHp;
        [SerializeField, ReadOnlyInspector] private int currentArmor;
        [SerializeField, ReadOnlyInspector] private int currentActionPoints;
        [SerializeField] private bool isBlocked;

        public int CurrentActionPoints
        {
            get => currentActionPoints;
            protected set
            {
                var previousValue = currentActionPoints;
                currentActionPoints = Math.Max(0, value);
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
        [field: SerializeField] public UnityEvent<bool, bool> CanBlockChanged { get; private set; }

        public int MaxHp => maxHp;

        public int CurrentHp
        {
            get => currentHp;
            private set
            {
                var before = currentHp;
                currentHp = Math.Min(Math.Max(0, value), maxHp);
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

        public bool IsBlocked
        {
            get => isBlocked;
            private set
            {
                var before = isBlocked;
                isBlocked = value;
                CanBlockChanged.Invoke(before, isBlocked);
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

        protected virtual void Awake()
        {
            currentHp = maxHp;
            currentArmor = initialArmor;
            currentActionPoints = initialActionPoints;
            movementLogic = GetComponent<UnitMovementLogic>();
            blockerSelector = new BaseUnitBlockerSelector();
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

        #region MoveCommand

        public void MoveTo([NotNull] Node target)
        {
            if (node.LeftUnit == this)
                node.LeftUnit = null;
            if (node.RightUnit == this)
                node.RightUnit = null;

            movementLogic.MoveTo(target.transform);
            CurrentActionPoints = movePointsModifier.Modify(CurrentActionPoints);
        }

        public bool CanMoveTo([NotNull] Node target)
        {
            return OwnerCondition()
                   && SizeCondition()
                   && LineCondition()
                   && ActionPointsCondition(movePointsModifier, CurrentActionPoints);

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
                       && (int) Passability <= (int) line.LineType;
            }

            bool OwnerCondition()
            {
                return target.Owner == null || target.Owner == Owner || target.Owner != Owner && target.AllIsFree;
            }
        }

        #endregion MoveCommand

        #region IAliveImplimitation
        public virtual void TakeDamage(Hit hit)
        {
            var blockedDamage = Math.Min(hit.Damage, CurrentArmor);

            var notBlockedDamage = hit.IsPenetrating ? hit.Damage : hit.Damage - blockedDamage;
            if (notBlockedDamage != 0)
                CurrentHp -= notBlockedDamage;
            if (IsBlocked)
            {
                IsBlocked = false;
                if (hit.Source is Unit enemy)
                {
                    UnitsController.ExecuteCommand(new ContrAttackCommand(this, enemy), false);
                }
            }
        }

        public virtual void HealMe(int healAmount)
        {
            if (healAmount < 0)
                throw new ArgumentException($"{nameof(healAmount)} > 0 !");
            CurrentHp += healAmount;
        }
        #endregion

        #region AttackCommand
        public virtual bool CanAttack([NotNull] Unit unit)
        {
            var line = node.GetLine(unit.node);
            return !AttackLocked &&
                   unit.basePlayer != basePlayer
                   && line != null
                   && (int) Passability <= (int) line.LineType
                   && ActionPointsCondition(attackPointsModifier, CurrentActionPoints);
        }

        public virtual void Attack([NotNull] Unit enemy)
        {
            // если нет соседа, то тогда просто атаковать
            if (!enemy.TryGetNeighbour(out var neighbour))
                AttackUnitButIgnoreBlock(neighbour);
            // иначе выбрать того, кто будет блокировать урон
            else
            {
                var selectedUnit = blockerSelector.SelectBlocker(enemy, neighbour);
                if (selectedUnit == enemy)
                    AttackUnitButIgnoreBlock(enemy);
                else
                {
                    UnitsController.ExecuteCommand(new BlockAttackCommand(this, selectedUnit));
                }
            }
        }

        public void AttackUnitButIgnoreBlock([NotNull] Unit enemy)
        {
            var enemyNode = enemy.node;
            MeleeAttack(enemy);

            if (enemy.IsDied && enemyNode.AllIsFree)
                UnitsController.ExecuteCommand(new MoveCommand(this, node, enemyNode));
        }

        protected void MeleeAttack(IAlive target)
        {
            target.TakeDamage(new Hit(MeleeDamage, this, target));
            CurrentActionPoints = attackPointsModifier.Modify(CurrentActionPoints);
        }

        public virtual bool CanAttack([NotNull] Edge edge) => false;

        public virtual void Attack([NotNull] Edge edge) { }
        #endregion
        
        public virtual bool CanContrAttack([NotNull] Unit enemy)
        {
            return CanAttack(enemy)
                   && MeleeDamage > 0;
        }
        
        public virtual void ContrAttack([NotNull] Unit enemy)
        {
            var contrAttackDamage = contrAttackDamageModifier
                ? contrAttackDamageModifier.Modify(MeleeDamage)
                : MeleeDamage;
            enemy.TakeDamage(new Hit(contrAttackDamage, this, enemy));
        }

        public virtual bool CanBlock()
        {
            return ActionPointsCondition(blockPointsModifier, CurrentActionPoints);
        }

        public virtual void EnableBlock()
        {
            IsBlocked = true;
            CurrentActionPoints = blockPointsModifier
                ? blockPointsModifier.Modify(CurrentActionPoints)
                : CurrentActionPoints = 0;
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

        public virtual void OnReplenish()
        {
            CurrentActionPoints = initialActionPoints;
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

        protected static bool ActionPointsCondition(IntModifier modifier, int actionPoints)
        {
            return actionPoints > 0 && modifier != null && modifier.Modify(actionPoints) >= 0;
        }
    }
}