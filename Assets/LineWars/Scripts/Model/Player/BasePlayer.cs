using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Extensions.Attributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace LineWars.Model
{
    /// <summary>
    /// класс, содержащий всю логику, которая объединяет ИИ и игрока
    /// </summary>
    public abstract class BasePlayer : MonoBehaviour, IActor
    {
        [field: SerializeField, ReadOnlyInspector]
        public int Index { get; set; }

        [SerializeField, ReadOnlyInspector] private NationType nationType;
        [SerializeField, Min(0)] private int money;

        public SpawnInfo SpawnInfo { get; set; }
        public PhaseType CurrentPhase { get; private set; }

        private HashSet<Owned> myOwned;
        private Nation nation;


        public event Action<int, int> CurrentMoneyChanged;
        public event Action<PhaseType> TurnStarted;
        public event Action<PhaseType> TurnEnded;

        public NationType NationType
        {
            get => nationType;
            set
            {
                nationType = value;
                nation = NationHelper.GetNationByType(nationType);
            }
        }

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

        protected virtual void Awake()
        {
            myOwned = new HashSet<Owned>();
        }

        public bool CanSpawnUnit(Node node, UnitType unitType)
        {
            if (unitType == UnitType.None) return false;

            var unit = GetUnitPrefab(unitType);
            return NodeConditional() && MoneyConditional() && BasePlayerUtility.CanSpawnUnit(node, unit);

            bool NodeConditional()
            {
                return node.IsBase;
            }

            bool MoneyConditional()
            {
                return CurrentMoney - unit.Cost > 0;
            }
        }

        public void SpawnUnit(Node node, UnitType unitType)
        {
            var unitPrefab = GetUnitPrefab(unitType);
            var unit = BasePlayerUtility.CreateUnitForPlayer(this, node, unitPrefab);
            CurrentMoney -= unit.Cost;
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
                }
            }
            else
            {
                myOwned.Add(owned);
            }
        }

        public void RemoveOwned(Owned owned)
        {
            myOwned.Remove(owned);
        }

        public Unit GetUnitPrefab(UnitType unitType) => nation.GetUnitPrefab(unitType);

        public void ExecuteTurn(PhaseType phaseType)
        {
            switch (phaseType)
            {
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
            }

            Debug.LogWarning
            ($"Phase.{phaseType} is not implemented in \"CanExecuteTurn\"! "
             + "Change IActor to acommodate for this phase!");
            return false;
        }

        public virtual void EndTurn()
        {
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

        #endregion
    }
}