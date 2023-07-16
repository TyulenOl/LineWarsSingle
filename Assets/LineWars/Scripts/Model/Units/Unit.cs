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
        
        [NotNull] private Node myNode;

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

        public void Initialize(Node myNode)
        {
            this.myNode = myNode;
        }
        
        public void MoveTo(Node target)
        {
            MovementLogic.MoveTo(target.transform); 
        }

        public bool IsCanMoveTo(Node target)
        {
            return SizeCondition() && LineCondition();

            bool SizeCondition()
            {
                return Size == UnitSize.Little && (target.LeftIsFree || target.RightIsFree)
                       || Size == UnitSize.Lage && (target.LeftIsFree && target.RightIsFree);
            }

            bool LineCondition()
            {
                var line = myNode.GetLine(target);
                return line != null
                       && line.LineType != LineType.Visibility
                       && line.LineType != LineType.Firing
                       && this.MinimaLineType <= line.LineType;
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