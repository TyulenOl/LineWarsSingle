using System;
using LineWars.Extensions.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars.Model
{
    public class Unit: MonoBehaviour,
        IAlive,
        IMovable,
        IHitHandler, 
        IHitCreator
    { 
        [SerializeField] private InitialBaseUnitCharacteristics initialBaseUnitCharacteristics;
        [SerializeField] [ReadOnlyInspector] private AllianceType allianceType;
        
        [SerializeField, ReadOnlyInspector] private Player owner;
        
        protected BaseUnitCharacteristics BaseUnitCharacteristics;
        protected UnitMovementLogic MovementLogic;
        private UnitDirection unitDirection;

        [HideInInspector] public UnityEvent<UnitSize, UnitDirection> UnitDirectionChance;  
        
        public int Hp => BaseUnitCharacteristics.Hp;
        public int Armor => BaseUnitCharacteristics.Armor;
        public int MeleeDamage => BaseUnitCharacteristics.MeleeDamage;
        public int Speed => BaseUnitCharacteristics.Speed;
        public UnitSize Size => BaseUnitCharacteristics.UnitSize;
        public LineType MinimaLineType => BaseUnitCharacteristics.MovingLineType;
        public AllianceType AllianceType => allianceType;
        public Player Owner => owner;

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

        private void OnEnable()
        {
            MovementLogic.TargetChanced += MovementLogicOnTargetChanced;
        }
        
        private void OnDisable()
        {
            MovementLogic.TargetChanced -= MovementLogicOnTargetChanced;
        }

        protected  void OnValidate()
        {
            if (GetComponent<UnitMovementLogic>() == null)
            {
                Debug.LogWarning($"у {name} не обнаружен компонент {nameof(UnitMovementLogic)}");
            }
        }
        
        private void MovementLogicOnTargetChanced(Transform before, Transform after)
        {
            //TODO
            //var point = GetComponent<Point>();
            
        }
        
        public void MoveTo(Point target)
        {
            
            MovementLogic.MoveTo(target.transform);
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