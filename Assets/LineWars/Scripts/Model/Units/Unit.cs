using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace LineWars.Model
{
    public class Unit : Owned,
        IAttackerVisitor,
        IAlive,
        IMovable,
        ITarget,
        IExecutor
    {
        [SerializeField] [Min(1)] private int currentHp;
        [SerializeField] [Min(0)] private int currentArmor;
        [SerializeField] [Min(0)] private int meleeDamage;
        [SerializeField] [Min(0)] private int currentSpeedPoints;
        [SerializeField] [Min(0)] private int visibility; 
        [SerializeField] private UnitSize unitSize;
        [SerializeField] private Passability passability;
        [SerializeField] private UnitDirection unitDirection;


        private UnitMovementLogic movementLogic;
        private Node node;


        [Header("Events")] public UnityEvent<UnitSize, UnitDirection> UnitDirectionChance;
        public UnityEvent<int, int> HpChanged;
        public UnityEvent<int, int> ArmorChanged;

        public int Hp
        {
            get => currentHp;
            private set
            {
                var before = currentHp;
                currentHp = Math.Max(0, value);
                HpChanged.Invoke(before, currentHp);
            }
        }

        public int Armor
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

        public UnitDirection UnitDirection
        {
            get => unitDirection;
            set
            {
                unitDirection = value;
                UnitDirectionChance?.Invoke(unitSize, unitDirection);
            }
        }

        private void Awake()
        {
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

        private void MovementLogicOnTargetChanced(Transform arg1, Transform arg2)
        {
            node = arg2.GetComponent<Node>();
        }

        public void Initialize(Node node)
        {
            this.node = node;
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
            var blockedDamage = Math.Min(hit.Damage, Armor);
            if (blockedDamage != 0)
                Armor -= blockedDamage;

            var notBlockedDamage = hit.Damage - blockedDamage;
            if (notBlockedDamage != 0)
                currentHp -= notBlockedDamage;
        }

        public void Attack([NotNull] Edge edge)
        {
            _Attack(edge);
        }

        public void Attack([NotNull] Unit unit)
        {
            _Attack(unit);
        }

        private void _Attack(IAlive target)
        {
            target.DealDamage(new Hit(MeleeDamage));
        }

        public bool IsCanAttack([NotNull] Unit unit)
        {
            var line = node.GetLine(unit.node);
            return line != null
                   && line.LineType >= LineType.Firing;
        }

        public bool IsCanAttack([NotNull] Edge edge)
        {
            return edge.LineType >= LineType.CountryRoad
                   && node.ContainsEdge(edge);
        }
    }
}