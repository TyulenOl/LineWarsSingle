using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace LineWars.Model
{
    public abstract class CBasePlayer: IActor, INumbered
    {
        public int Index { get; }
        
        private int money;
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
        
        /// <summary>
        /// Для оптимизации income всегда хешируется
        /// </summary>
        private int income;
        public int Income
        {
            get => income;
            set
            {
                var before = income;
                income = value;
                IncomeChanged?.Invoke(before, income);
            }
        }
        
        public CNode Base { get; }
        public PlayerRules Rules { get;}
        public PhaseType CurrentPhase { get; private set; }
        public Nation MyNation { get; }

        private HashSet<COwned> myOwned = new ();
        public IEnumerable<CNode> MyNodes => myOwned.OfType<CNode>();
        public IEnumerable<CComponentUnit> MyUnits => myOwned.OfType<CComponentUnit>();
        
        private bool isFirstReplenish = true;

        public event Action<PhaseType, PhaseType> TurnChanged;
        public event Action<COwned> OwnedAdded;
        public event Action<COwned> OwnedRemoved;
        public event Action<int, int> CurrentMoneyChanged;
        public event Action<int, int> IncomeChanged;
        public event Action Defeated; 
        public IReadOnlyCollection<COwned> OwnedObjects => myOwned;
        public bool IsMyOwn(COwned owned) => myOwned.Contains(owned);


        public CBasePlayer(SpawnInfo spawnInfo)
        {
            Index = spawnInfo.PlayerIndex;
            Base = spawnInfo.SpawnNode.Node;
            Rules = spawnInfo.SpawnNode.Rules ? spawnInfo.SpawnNode.Rules : PlayerRules.DefaultRules;

            CurrentMoney = Rules.StartMoney;
            Income = Rules.DefaultIncome;
            MyNation = spawnInfo.SpawnNode.Nation;
        }
        
        public bool CanSpawnPreset(UnitBuyPreset preset)
        {
            return NodeConditional() && MoneyConditional();

            bool NodeConditional()
            {
                return Base.AllIsFree;
            }

            bool MoneyConditional()
            {
                return CurrentMoney - preset.Cost >= 0;
            }
        }

        public void SpawnUnit(CNode node, UnitType unitType)
        {
            if (unitType == UnitType.None) return;
            var unitPrefab = GetUnitPrefab(unitType);
            BasePlayerUtility.CreateUnitForPlayer(this, node, unitPrefab);
        }

        public void SpawnPreset(UnitBuyPreset unitPreset)
        {
            SpawnUnit(Base, unitPreset.FirstUnitType);
            SpawnUnit(Base, unitPreset.SecondUnitType);
            CurrentMoney -= unitPreset.Cost;
        }

        public void AddOwned([NotNull] COwned owned)
        {
            if (owned == null) throw new ArgumentNullException(nameof(owned));

            if (owned.Owner != null && owned.Owner == this)
                return;
            if (owned.Owner != null && owned.Owner != this)
            {
                owned.Owner.RemoveOwned(owned);
            }

            switch (owned)
            {
                case CNode node:
                    BeforeAddOwned(node);
                    break;
                case CComponentUnit unit:
                    BeforeAddOwned(unit);
                    break;
            }

            myOwned.Add(owned);
            OwnedAdded?.Invoke(owned);
        }

        protected virtual void BeforeAddOwned(CNode node)
        {
            var nodeIncome = GetMyIncomeFromNode(node);
            if (!node.IsDirty) CurrentMoney += GetMyCapturingMoneyFromNode(node);
            Income += nodeIncome;
        }

        public int GetMyIncomeFromNode(CNode node)
        {
            return Mathf.RoundToInt(Rules.IncomeModifier.Modify(node.BaseIncome));
        }
        
        public int GetMyCapturingMoneyFromNode(CNode node)
        {
            return Rules.MoneyForFirstCapturingNode + GetMyIncomeFromNode(node);
        }

        protected virtual void BeforeAddOwned(CComponentUnit unit)
        {
            
        }

        public void RemoveOwned([NotNull] COwned owned)
        {
            if (owned == null) throw new ArgumentNullException(nameof(owned));

            if (!myOwned.Contains(owned)) return;

            switch (owned)
            {
                case CNode node:
                    BeforeRemoveOwned(node);
                    break;
                case CComponentUnit unit:
                    BeforeRemoveOwned(unit);
                    break;
            }
            
            myOwned.Remove(owned);
            OwnedRemoved?.Invoke(owned);
        }
        
        protected virtual void BeforeRemoveOwned(CNode node)
        {
            Income -= Mathf.RoundToInt(Rules.IncomeModifier.Modify(node.BaseIncome));

            if (node == Base)
            {
                Defeat();
            }
        }

        protected virtual void BeforeRemoveOwned(CComponentUnit unit)
        {
        }

        public void Defeat()
        {
            OnDefeat();
            Defeated?.Invoke();
        }
        protected virtual void OnDefeat()
        {
            //TODO
        }
        
        public ComponentUnit GetUnitPrefab(UnitType unitType) => MyNation.GetUnitPrefab(unitType);

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
            if (isFirstReplenish)
            {
                isFirstReplenish = false;
                return;
            }
            CurrentMoney += Income;
            foreach (var owned in OwnedObjects)
                owned.Replenish();
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