using System;
using System.Diagnostics.CodeAnalysis;
using LineWars.Controllers;
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
        [SerializeField] [Min(0)] private int initialHp;
        [SerializeField] [Min(0)] private int initialArmor;
        [SerializeField] [Min(0)] private int meleeDamage;
        [SerializeField] [Min(0)] private int initialSpeedPoints;
        [SerializeField] [Min(0)] private int visibility; 
        [SerializeField] private UnitSize unitSize;
        [SerializeField] private Passability passability;
        
        private int currentHp;
        private int currentArmor;
        private int currentSpeedPoints;

        private UnitMovementLogic movementLogic;
        private Node node;


        [field: Header("Events")] 
        [field: SerializeField] public UnityEvent<UnitSize, UnitDirection> UnitDirectionChance { get; private set; }
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

        public int Visibility => visibility;
        public UnitSize Size => unitSize;
        public Passability Passability => passability;
        public Node Node => node;

        public UnitDirection UnitDirection => node.LeftUnit == this ? UnitDirection.Left : UnitDirection.Right;

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
            movementLogic.TargetChanced += MovementLogicOnTargetChanced;
        }

        private void OnDisable()
        {
            movementLogic.TargetChanced -= MovementLogicOnTargetChanced;
        }

        private void MovementLogicOnTargetChanced(Transform arg1, Transform arg2)
        {
            CurrentSpeedPoints--;
            
            node = arg2.GetComponent<Node>();
            if (Size == UnitSize.Large)
            {
                node.LeftUnit = this;
                node.RightUnit = this;
            }
            else if (node.LeftIsFree)
            {
                node.LeftUnit = this;
                UnitDirectionChance?.Invoke(unitSize, UnitDirection.Left);
            }
            else
            {
                node.RightUnit = this;
                UnitDirectionChance?.Invoke(unitSize, UnitDirection.Right);
            }
        }

        public void Initialize(Node node, UnitDirection direction)
        {
            this.node = node;
            UnitDirectionChance?.Invoke(unitSize, direction);
        }

        public void MoveTo([NotNull] Node target)
        {
            movementLogic.MoveTo(target.transform);
        }

        public bool IsCanMoveTo([NotNull] Node target)
        {
            return SizeCondition() && LineCondition() && SpeedConditional();

            bool SizeCondition()
            {
                return Size == UnitSize.Little && (target.LeftIsFree || target.RightIsFree)
                       || Size == UnitSize.Large && (target.LeftIsFree && target.RightIsFree);
            }

            bool LineCondition()
            {
                var line = node.GetLine(target);
                return line != null
                       && line.LineType != LineType.Visibility
                       && line.LineType != LineType.Firing
                       && (int) Passability >= (int) line.LineType;
            }

            bool SpeedConditional()
            {
                return CurrentSpeedPoints > 0;
            }
        }

        public void DealDamage(Hit hit)
        {
            var blockedDamage = Math.Min(hit.Damage, CurrentArmor);
            if (blockedDamage != 0)
                CurrentArmor -= blockedDamage;

            var notBlockedDamage = hit.Damage - blockedDamage;
            if (notBlockedDamage != 0)
                currentHp -= notBlockedDamage;
        }

        public void Attack([NotNull] Edge edge)
        {
            _Attack(edge);
        }

        public void Attack([NotNull] Unit enemy)
        {
            var enemyNode = enemy.node;
            _Attack(enemy);
            
            if (enemy.IsDied)
                UnitsController.ExecuteCommand(new MoveCommand(this, node, enemyNode));
        }

        private void _Attack(IAlive target)
        {
            target.DealDamage(new Hit(MeleeDamage));
            
        }

        public bool IsCanAttack([NotNull] Unit unit)
        {
            var line = node.GetLine(unit.node);
            return unit.owner != owner 
                   && line != null
                   && (int) Passability >= (int) line.LineType;
        }

        public bool IsCanAttack([NotNull] Edge edge)
        {
            return edge.LineType >= LineType.CountryRoad
                   && node.ContainsEdge(edge);
        }

        private void OnDied()
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
        }

        public void OnTurnEnd()
        {
            CurrentHp = initialHp;
            CurrentArmor = initialArmor;
            CurrentSpeedPoints = initialSpeedPoints;
        }
    }
}