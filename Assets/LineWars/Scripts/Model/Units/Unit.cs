using System;
using System.Diagnostics.CodeAnalysis;
using LineWars.Extensions.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Model
{
    public class Unit: Owned,
        IAlive,
        IHitHandler,
        IHitCreator
    { 
        [SerializeField] private InitialBaseUnitCharacteristics initialBaseUnitCharacteristics;
        [SerializeField] [ReadOnlyInspector] private AllianceType allianceType;
        
        protected BaseUnitCharacteristics BaseUnitCharacteristics;
        protected UnitMovementLogic MovementLogic;
        private UnitDirection unitDirection;
        
        [NotNull] private Point myPoint;

        [HideInInspector] public UnityEvent<UnitSize, UnitDirection> UnitDirectionChance;  
        
        public int Hp => BaseUnitCharacteristics.Hp;
        public int Armor => BaseUnitCharacteristics.Armor;
        public int MeleeDamage => BaseUnitCharacteristics.MeleeDamage;
        public int Speed => BaseUnitCharacteristics.Speed;
        public UnitSize Size => BaseUnitCharacteristics.UnitSize;
        public LineType MinimaLineType => BaseUnitCharacteristics.MovingLineType;
        public AllianceType AllianceType => allianceType;

        public UnitDirection UnitDirection
        {
            get => unitDirection;
            set
            {
                unitDirection = value;
                UnitDirectionChance?.Invoke(BaseUnitCharacteristics.UnitSize, unitDirection);
            }
        }

        private void Awake()
        {
            BaseUnitCharacteristics = new BaseUnitCharacteristics(initialBaseUnitCharacteristics);
            MovementLogic = GetComponent<UnitMovementLogic>();
        }
        
        protected void OnValidate()
        {
            if (GetComponent<UnitMovementLogic>() == null)
            {
                Debug.LogError($"у {name} не обнаружен компонент {nameof(UnitMovementLogic)}");
            }
        }

        public void Initialize(Point myPoint)
        {
            this.myPoint = myPoint;
        }
        
        public void MoveTo(Point target)
        {
            MovementLogic.MoveTo(target.transform);
        }

        public bool IsCanMoveTo(Point target)
        {
            return SizeCondition() && LineCondition();

            bool SizeCondition()
            {
                return Size == UnitSize.Little && (target.LeftIsFree || target.RightIsFree)
                       || Size == UnitSize.Lage && (target.LeftIsFree && target.RightIsFree);
            }

            bool LineCondition()
            {
                return myPoint.HasLine(target);
            }
        }

        public void Accept(Hit hit)
        {
            throw new NotImplementedException();
        }

        public Hit GenerateHit()
        {
            throw new NotImplementedException();
        }
    }
}