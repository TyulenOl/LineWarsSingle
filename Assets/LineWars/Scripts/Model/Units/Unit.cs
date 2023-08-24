using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LineWars.Extensions.Attributes;
using UnityEngine;
using UnityEngine.Events;
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

        [SerializeField, Min(0)] protected int initialArmor;
        [SerializeField, Min(0)] protected int meleeDamage;
        [SerializeField, Min(0)] protected int visibility;
        [SerializeField, Min(0)] protected int cost;
        [SerializeField] protected bool attackLocked;
        [SerializeField] protected bool occupyPointAfterMeleeAttack;
        [SerializeField] protected bool isPenetratingMeleeAttack;

        [SerializeField] protected UnitType unitType;
        [SerializeField] protected UnitSize unitSize;
        [SerializeField] protected LineType movementLineType;
        [SerializeField] protected CommandPriorityData priorityData;

        [Header("Action Points Settings")] 
        [SerializeField] [Min(0)] protected int initialActionPoints;

        [SerializeField] protected IntModifier attackPointsModifier;
        [SerializeField] protected IntModifier movePointsModifier;
        [SerializeField] protected IntModifier blockPointsModifier;

        [Header("Other")] 
        [SerializeField] protected IntModifier contrAttackDamageModifier;

        private UnitMovementLogic movementLogic;
        private IUnitBlockerSelector blockerSelector;

        [Header("DEBUG")] [SerializeField, ReadOnlyInspector]
        private Node node;

        [SerializeField, ReadOnlyInspector] private UnitDirection unitDirection;
        [SerializeField, ReadOnlyInspector] private int currentHp;
        [SerializeField, ReadOnlyInspector] private int currentArmor;
        [SerializeField, ReadOnlyInspector] private int currentActionPoints;
        [SerializeField] private bool isBlocked;
        
        [field: Header("Events")]
        [field: SerializeField] public UnityEvent<UnitSize, UnitDirection> UnitDirectionChange { get; private set; }

        [field: SerializeField] public UnityEvent<int, int> HpChanged { get; private set; }
        [field: SerializeField] public UnityEvent<int, int> ArmorChanged { get; private set; }
        [field: SerializeField] public UnityEvent<Unit> Died { get; private set; }
        [field: SerializeField] public UnityEvent<int, int> ActionPointChanged { get; private set; }
        [field: SerializeField] public UnityEvent<bool, bool> CanBlockChanged { get; private set; }
        
        public int CurrentActionPoints
        {
            get => currentActionPoints;
            protected set
            {
                var previousValue = currentActionPoints;
                currentActionPoints = Math.Max(0, value);
                ActionPointChanged.Invoke(previousValue, currentActionPoints);
            }
        }
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
        public LineType MovementLineType => movementLineType;
        public Node Node => node;
        public CommandPriorityData CommandPriorityData => priorityData;
        public IntModifier BlockPointsModifier => blockPointsModifier;
        public IntModifier AttackPointsModifier => attackPointsModifier;
        public IntModifier MovePointsModifier => movePointsModifier;
        public bool CanMoveOnLineWithType(LineType lineType) => lineType >= MovementLineType;

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

        protected override void OnDisable()
        {
            base.OnDisable();
            movementLogic.MovementIsOver -= MovementLogicOnMovementIsOver;
        }

        private void MovementLogicOnMovementIsOver(Transform arg2)
        {
            
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
            
            AssignNewNode(target);
            
            movementLogic.MoveTo(target.transform);
            CurrentActionPoints = movePointsModifier.Modify(CurrentActionPoints);
        }

        private void AssignNewNode(Node target)
        {
            node = target;
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

        public bool CanMoveTo([NotNull] Node target)
        {
            return Node != target
                   && OwnerCondition()
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
                       && CanMoveOnLineWithType(line.LineType);
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

            if (hit is {Source: Unit enemy, IsRangeAttack: false})
                UnitsController.ExecuteCommand(new ContrAttackCommand(this, enemy), false);
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
            if (unit == null) throw new ArgumentNullException(nameof(unit));
           
            return BaseMeleeAttackCondition(unit)
                   && ActionPointsCondition(attackPointsModifier, CurrentActionPoints);
        }
        public virtual void Attack([NotNull] Unit enemy)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));

            // если нет соседа, то тогда просто атаковать
            if (!enemy.TryGetNeighbour(out var neighbour))
                AttackUnitButIgnoreBlock(enemy);
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
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            var enemyNode = enemy.node;
            MeleeAttack(enemy);

            if (enemy.IsDied && enemyNode.AllIsFree && occupyPointAfterMeleeAttack)
                UnitsController.ExecuteCommand(new MoveCommand(this, node, enemyNode));
        }

        private void MeleeAttack(IAlive target)
        {
            target.TakeDamage(new Hit(MeleeDamage, this, target, isPenetratingMeleeAttack));
            CurrentActionPoints = attackPointsModifier.Modify(CurrentActionPoints);
        }

        public virtual bool CanAttack([NotNull] Edge edge) => false;

        public virtual void Attack([NotNull] Edge edge) { }
        #endregion

        #region ContrAttackCommand
        public virtual bool CanContrAttack([NotNull] Unit enemy)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            return IsBlocked 
                   && BaseMeleeAttackCondition(enemy)
                   && MeleeDamage > 0;
        }
        
        public virtual void ContrAttack([NotNull] Unit enemy)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            var contrAttackDamage = contrAttackDamageModifier
                ? contrAttackDamageModifier.Modify(MeleeDamage)
                : MeleeDamage;
            enemy.TakeDamage(new Hit(contrAttackDamage, this, enemy));
            IsBlocked = false;
        }
        #endregion
        
        #region BlockCommand
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
        #endregion
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
            IsBlocked = false;
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

        private bool BaseMeleeAttackCondition(Unit unit)
        {
            var line = node.GetLine(unit.node);
            return !attackLocked 
                   && unit.Owner != Owner
                   && line != null
                   && CanMoveOnLineWithType(line.LineType);
        }
        
        protected static bool ActionPointsCondition(IntModifier modifier, int actionPoints)
        {
            return actionPoints > 0 && modifier != null && modifier.Modify(actionPoints) >= 0;
        }
        
        public virtual IEnumerable<(ITarget,CommandType)> GetAllAvailableTargets()
        {
            return GetAllAvailableTargetsInRange((uint)currentActionPoints);
        }

        protected IEnumerable<(ITarget, CommandType)> GetAllAvailableTargetsInRange(uint range)
        {
            foreach (var e in Graph.GetNodesInRange(node, range))
            {
                foreach (var target in e.GetTargetsWithMe())
                {
                    yield return (target, UnitsController.Instance.GetCommandTypeBy(this, target));
                }
            }
        }

        public virtual CommandType GetAttackTypeBy(IAlive target)
        {
            return CommandType.Attack;
        }
    }
}