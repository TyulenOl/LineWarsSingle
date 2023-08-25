using System;
using System.Collections.Generic;
using LineWars.Extensions.Attributes;
using UnityEngine;
using UnityEngine.Events;
using LineWars.Controllers;

namespace LineWars.Model
{
    /// <summary>
    /// класс, содержащий всю логику, которая объединяет ИИ и игрока
    /// </summary>
    public abstract class BasePlayer : MonoBehaviour, IActor
    {
        [field: SerializeField, ReadOnlyInspector] public int Index { get; set; }

        [SerializeField, ReadOnlyInspector] private NationType nationType;
        [SerializeField, ReadOnlyInspector] private int money;
        [field: SerializeField, ReadOnlyInspector] public Spawn Base { get; set; }
        public PhaseType CurrentPhase { get; private set; }

        private HashSet<Owned> myOwned;
        protected Nation nation;

        public event Action<int, int> CurrentMoneyChanged;

        public NationType NationType
        {
            get => nationType;
            set
            {
                nationType = value;
                nation = NationHelper.GetNationByType(nationType);
            }
        }

        public event Action<PhaseType, PhaseType> TurnChanged;
        public event Action<Owned> OwnedAdded;
        public event Action<Owned> OwnedRemoved;
        public IReadOnlyCollection<Owned> OwnedObjects => myOwned;
        public bool IsMyOwn(Owned owned) => myOwned.Contains(owned);

        public int CurrentMoney
        {
            get => money;
            set
            {
                var before = money;
                money = Math.Max(0, value);

                if (before != money)
                    CurrentMoneyChanged?.Invoke(before, money);
                
            }
        }

        public int Income => 20; // Временно

        protected virtual void Awake()
        {
            myOwned = new HashSet<Owned>();
        }

        protected virtual void Start()
        {
            if (PhaseManager.Instance != null)
            {
                PhaseManager.Instance.RegisterActor(this);
                Debug.Log($"{name} registered");
            }
        }

        protected virtual void OnEnable()
        {
            
        }
        
        protected virtual void OnDisable()
        {
            
        }

        public bool CanSpawnUnit(Node node, UnitBuyPreset preset)
        {
            return NodeConditional() && MoneyConditional();

            bool NodeConditional()
            {
                return node.IsBase && node.AllIsFree;
            }

            bool MoneyConditional()
            {
                return CurrentMoney - preset.Cost >= 0;
            }
        }

        public void SpawnUnit(Node node, UnitType unitType)
        {
            if(unitType == UnitType.None) return;
            var unitPrefab = GetUnitPrefab(unitType);
            BasePlayerUtility.CreateUnitForPlayer(this, node, unitPrefab);
        }

        public void SpawnPreset(Node node, UnitBuyPreset unitPreset)
        {
            SpawnUnit(node,unitPreset.FirstUnitType);
            SpawnUnit(node,unitPreset.SecondUnitType);
            CurrentMoney -= unitPreset.Cost;
        }

        public void AddOwned(Owned owned)
        {
            if (owned == null) return;
            if (owned.Owner != null)
            {
                if (owned.Owner != this)
                {
                    owned.Owner.RemoveOwned(owned);
                    myOwned.Add(owned);
                    OwnedAdded?.Invoke(owned);
                }
            }
            else
            {
                myOwned.Add(owned);
                OwnedAdded?.Invoke(owned);
            }
        }

        public void RemoveOwned(Owned owned)
        {
            if (!myOwned.Contains(owned)) return;
            myOwned.Remove(owned);
            OwnedRemoved?.Invoke(owned);
        }

        public Unit GetUnitPrefab(UnitType unitType) => nation.GetUnitPrefab(unitType);

        public void ExecuteTurn(PhaseType phaseType)
        {
            var previousPhase = CurrentPhase;
            switch (phaseType)
            {
                case PhaseType.Replenish:
                    ExecuteReplenish();
                    break;
                case PhaseType.Idle:
                    ExecuteIdle();
                    break;
                case PhaseType.Buy:
                    ExecuteBuy();
                    break;
                case PhaseType.Artillery:
                    ExecuteArtillery();
                    break;
                case PhaseType.Fight:
                    ExecuteFight();
                    break;
                case PhaseType.Scout:
                    ExecuteScout();
                    break;
                default:
                    Debug.LogWarning($"Phase.{phaseType} is not implemented in \"ExecuteTurn\"! "
                                     + "Change IActor to acommodate for this phase!");
                    break;
            }

            CurrentPhase = phaseType;
            TurnChanged?.Invoke(previousPhase, CurrentPhase);
        }

        public bool CanExecuteTurn(PhaseType phaseType)
        {
            switch (phaseType)
            {
                case PhaseType.Idle:
                    return true;
                case PhaseType.Buy:
                    return CanExecuteBuy();
                case PhaseType.Artillery:
                    return CanExecuteArtillery();
                case PhaseType.Fight:
                    return CanExecuteFight();
                case PhaseType.Scout:
                    return CanExecuteScout();
                case PhaseType.Replenish:
                    return CanExecuteReplenish();
            }

            Debug.LogWarning
            ($"Phase.{phaseType} is not implemented in \"CanExecuteTurn\"! "
             + "Change IActor to acommodate for this phase!");
            return false;
        }

        #region Turns

        public virtual void ExecuteBuy()
        {
        }

        public virtual void ExecuteArtillery()
        {
        }

        public virtual void ExecuteFight()
        {
        }

        public virtual void ExecuteScout()
        {
        }


        public virtual void ExecuteIdle()
        {
        }

        public virtual void ExecuteReplenish()
        {
        }

        #endregion

        #region Check Turns

        public virtual bool CanExecuteBuy()
        {
            return false;
        }

        public virtual bool CanExecuteArtillery()
        {
            return false;
        }

        public virtual bool CanExecuteFight()
        {
            return false;
        }

        public virtual bool CanExecuteScout()
        {
            return false;
        }

        public virtual bool CanExecuteReplenish()
        {
            return false;
        }

        #endregion
    }
}