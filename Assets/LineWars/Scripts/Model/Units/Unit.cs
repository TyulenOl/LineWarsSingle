using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LineWars.Controllers;
using LineWars.Extensions.Attributes;
using UnityEngine;
using UnityEngine.Events;

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
        [SerializeField] private string unitName;

        [SerializeField, Min(0)] private int maxHp;
        [SerializeField, Min(0)] protected int initialArmor;
        [SerializeField, Min(0)] protected int visibility;


        [SerializeField] protected UnitType unitType;
        [SerializeField] protected UnitSize unitSize;
        [SerializeField] protected LineType movementLineType;
        [SerializeField] protected CommandPriorityData priorityData;

        [Header("Attack Settings")]
        [SerializeField] protected bool attackLocked = false;
        [SerializeField, Min(0)] private int damage;
        [SerializeField] protected bool isPenetratingDamage = false;
        /// <summary>
        /// указывет на то, нужно ли захватывать точку после атаки
        /// </summary>
        [SerializeField] protected bool onslaught = false;
        /// <summary>
        /// юнит с этим флагом всегда контратакует
        /// </summary>
        [SerializeField] protected bool protection = false;

        [Header("ContrAttack Settings")]
        [SerializeField] protected bool canContrAttack;

        [SerializeField] protected IntModifier contrAttackDamageModifier;


        [Header("Action Points Settings")] 
        [SerializeField] [Min(0)] protected int initialActionPoints;

        [SerializeField] protected IntModifier attackPointsModifier;
        [SerializeField] protected IntModifier movePointsModifier;
        [SerializeField] protected IntModifier blockPointsModifier;

        [Header("Sfx Settings")] 
        [SerializeField] private SFXData moveSFX;
        [SerializeField] private SFXData attackSFX;

        [Header("DEBUG")]
        [SerializeField, ReadOnlyInspector] private Node node;
        [SerializeField, ReadOnlyInspector] private UnitDirection unitDirection;
        [SerializeField, ReadOnlyInspector] private int currentHp;
        [SerializeField, ReadOnlyInspector] private int currentArmor;
        [SerializeField, ReadOnlyInspector] private int currentActionPoints;
        [SerializeField, ReadOnlyInspector] private bool isBlocked;


        [field: Header("Events")]
        [field: SerializeField] public UnityEvent<UnitSize, UnitDirection> UnitDirectionChange { get; private set; }
        [field: SerializeField] public UnityEvent<int, int> HpChanged { get; private set; }
        [field: SerializeField] public UnityEvent<int, int> ArmorChanged { get; private set; }
        [field: SerializeField] public UnityEvent<Unit> Died { get; private set; }
        [field: SerializeField] public UnityEvent<int, int> ActionPointsChanged { get; private set; }
        [field: SerializeField] public UnityEvent<bool, bool> CanBlockChanged { get; private set; }
        [field: SerializeField] public UnityEvent ActionCompleted { get; private set; }

        private UnitMovementLogic movementLogic;
        private IUnitBlockerSelector blockerSelector;

        public string UnitName => unitName;

        public bool CanDoAnyAction => CurrentActionPoints > 0;

        public int CurrentActionPoints
        {
            get => currentActionPoints;
            protected set
            {
                var previousValue = currentActionPoints;
                currentActionPoints = Math.Max(0, value);
                ActionPointsChanged.Invoke(previousValue, currentActionPoints);
            }
        }

        public int MaxHp => maxHp;
        public int Armor => initialArmor;

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
                currentArmor = Math.Max(0, value);
                ArmorChanged.Invoke(before, currentArmor);
            }
        }

        public bool IsBlocked
        {
            get => isBlocked;
            protected set
            {
                var before = isBlocked;
                isBlocked = value;
                if (before != isBlocked)
                    CanBlockChanged.Invoke(before, isBlocked);
            }
        }

        public int Damage => damage;
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
        public int Cost
        {
            get
            {
                Debug.LogWarning("Вы используете устаревшее свойство Cost! Ай-яй-яй!");
                return 1; // не использовать! устаревшее свойство (блин)
            }
        }

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
            
            AssignModifiers();

            void AssignModifiers()
            {
                if (attackPointsModifier == null)
                {
                    attackPointsModifier = DecreaseIntModifier.DecreaseOne;
                    Debug.LogWarning($"{nameof(attackPointsModifier)} is null on {name}");
                }

                if (contrAttackDamageModifier == null)
                {
                    contrAttackDamageModifier = MultiplyIntModifier.MultiplyOne;
                    Debug.LogWarning($"{nameof(contrAttackDamageModifier)} is null on {name}");
                }

                if (blockPointsModifier == null)
                {
                    blockPointsModifier = SetIntModifier.Set0;
                    Debug.LogWarning($"{nameof(blockPointsModifier)} is null on {name}");
                }

                if (movePointsModifier == null)
                {
                    movePointsModifier = DecreaseIntModifier.DecreaseOne;
                    Debug.LogWarning($"{nameof(movePointsModifier)} is null on {name}");
                }
            }
        }
        
        
        public void Initialize(Node node, UnitDirection direction)
        {
            this.node = node;
            UnitDirection = direction;
        }

        #region MoveCommand

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

        public void MoveTo([NotNull] Node target)
        {
            if (node.LeftUnit == this)
                node.LeftUnit = null;
            if (node.RightUnit == this)
                node.RightUnit = null;

            InspectNodeForCallback();
            AssignNewNode();

            movementLogic.MoveTo(target.transform);
            CurrentActionPoints = movePointsModifier.Modify(CurrentActionPoints);
            ActionCompleted.Invoke();
            SfxManager.Instance.Play(moveSFX);
            
            void InspectNodeForCallback()
            {
                if (target.Owner == null)
                {
                    OnCapturingFreeNode();
                    return;
                }

                if (target.Owner != this.Owner)
                {
                    OnCapturingEnemyNode();
                    if (target.IsBase)
                        OnCapturingEnemyBase();
                }
            }
            void AssignNewNode()
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
        }
        
        #endregion MoveCommand

        #region IAliveImplimitation

        public virtual void TakeDamage(Hit hit)
        {
            var blockedDamage = Math.Min(hit.Damage, CurrentArmor);

            var notBlockedDamage = hit.IsPenetrating ? hit.Damage : hit.Damage - blockedDamage;
            if (notBlockedDamage != 0)
                CurrentHp -= notBlockedDamage;

            if (!IsDied && hit is {Source: Unit enemy, IsRangeAttack: false})
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

        public virtual bool CanAttack(Unit unit, Node node)
        {
            if (unit == null) throw new ArgumentNullException(nameof(unit));

            return BaseMeleeAttackCondition(unit, node)
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

            if (enemy.IsDied && enemyNode.AllIsFree && onslaught)
                UnitsController.ExecuteCommand(new MoveCommand(this, node, enemyNode));
        }

        private void MeleeAttack(IAlive target)
        {
            target.TakeDamage(new Hit(Damage, this, target, isPenetratingDamage));
            CurrentActionPoints = attackPointsModifier.Modify(CurrentActionPoints);
            ActionCompleted.Invoke();
            SfxManager.Instance.Play(attackSFX);
        }

        public virtual bool CanAttack([NotNull] Edge edge) => false;
        public virtual bool CanAttack(Edge edge, Node node) => false;

        public virtual void Attack([NotNull] Edge edge)
        {
        }
        
        private bool BaseMeleeAttackCondition(Unit unit) => BaseMeleeAttackCondition(unit, node);

        private bool BaseMeleeAttackCondition(Unit unit, Node node)
        {
            var line = node.GetLine(unit.Node);
            return !attackLocked
                   && Damage > 0
                   && unit.Owner != Owner
                   && line != null
                   && CanMoveOnLineWithType(line.LineType);
        }

        #endregion

        #region ContrAttackCommand

        public virtual bool CanContrAttack([NotNull] Unit enemy)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            return canContrAttack 
                   && (IsBlocked || protection)
                   && contrAttackDamageModifier.Modify(Damage) > 0
                   && BaseMeleeAttackCondition(enemy);
        }

        public virtual void ContrAttack([NotNull] Unit enemy)
        {
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            var contrAttackDamage = contrAttackDamageModifier.Modify(Damage);
          
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
            ActionCompleted.Invoke();
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

            Owner.RemoveOwned(this);
            Destroy(gameObject);
        }

        protected override void OnReplenish()
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
        

        protected static bool ActionPointsCondition(IntModifier modifier, int actionPoints)
        {
            return actionPoints > 0 && modifier != null && modifier.Modify(actionPoints) >= 0;
        }

        public virtual IEnumerable<(ITarget, CommandType)> GetAllAvailableTargets()
        {
            return GetAllAvailableTargetsInRange((uint) currentActionPoints + 1);
        }

        protected IEnumerable<(ITarget, CommandType)> GetAllAvailableTargetsInRange(uint range)
        {
            var visibilityEdges = new HashSet<Edge>();
            foreach (var e in Graph.GetNodesInRange(node, range))
            {
                foreach (var target in e.GetTargetsWithMe())
                    yield return (target, UnitsController.Instance.GetCommandTypeBy(this, target));
                foreach (var edge in node.Edges)
                {
                    if (visibilityEdges.Contains(edge))
                        continue;
                    visibilityEdges.Add(edge);
                    yield return (edge, UnitsController.Instance.GetCommandTypeBy(this, edge));
                }
            }
        }

        public virtual CommandType GetAttackTypeBy(IAlive target) => CommandType.MeleeAttack;

        #region CallBack

        protected virtual void OnCapturingEnemyBase()
        {
        }

        protected virtual void OnCapturingEnemyNode()
        {
        }

        protected virtual void OnCapturingFreeNode()
        {
        }

        #endregion
    }
}