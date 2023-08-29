using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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
        [field: SerializeField, ReadOnlyInspector]
        public int Index { get; private set; }

        [SerializeField, ReadOnlyInspector] private NationType nationType;
        [SerializeField, ReadOnlyInspector] private int money;
        [SerializeField, ReadOnlyInspector] private int score;

        [field: SerializeField, ReadOnlyInspector]
        public Spawn Base { get; private set; }

        [field: SerializeField, ReadOnlyInspector]
        public PlayerRules Rules { get; private set; }

        public PhaseType CurrentPhase { get; private set; }

        private HashSet<Owned> myOwned;
        protected Nation nation;

        /// <summary>
        /// Для оптимизации income всегда хешируется
        /// </summary>
        private int income;
        

        public NationType NationType
        {
            get => nationType;
            set
            {
                nationType = value;
                nation = NationHelper.GetNationByType(nationType);
            }
        }

        private IEnumerable<Node> MyNodes => myOwned.OfType<Node>();

        public event Action<PhaseType, PhaseType> TurnChanged;
        public event Action<Owned> OwnedAdded;
        public event Action<Owned> OwnedRemoved;
        public event Action<int, int> CurrentMoneyChanged;
        public event Action<int, int> IncomeChanged;
        public event Action<int, int> ScoreChanged;
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

        public int Score
        {
            get => score;
            set
            {
                var before = score;
                score = value;
                ScoreChanged?.Invoke(before, score);
            }
        }

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

        public virtual void Initialize(SpawnInfo spawnInfo)
        {
            name = $"{GetType().Name}{spawnInfo.PlayerIndex} {spawnInfo.SpawnNode.name}";
            Index = spawnInfo.PlayerIndex;
            Base = spawnInfo.SpawnNode;
            Rules = spawnInfo.SpawnNode.Rules ? spawnInfo.SpawnNode.Rules : PlayerRules.DefaultRules;

            CurrentMoney = Rules.StartMoney;
            Income = Rules.DefaultIncome;
            NationType = Rules.Nation;
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
            if (unitType == UnitType.None) return;
            var unitPrefab = GetUnitPrefab(unitType);
            BasePlayerUtility.CreateUnitForPlayer(this, node, unitPrefab);
        }

        public void SpawnPreset(Node node, UnitBuyPreset unitPreset)
        {
            SpawnUnit(node, unitPreset.FirstUnitType);
            SpawnUnit(node, unitPreset.SecondUnitType);
            CurrentMoney -= unitPreset.Cost;
        }

        public void AddOwned([NotNull] Owned owned)
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
                case Node node:
                    BeforeAddOwned(node);
                    break;
                case Unit unit:
                    BeforeAddOwned(unit);
                    break;
            }

            myOwned.Add(owned);
            OwnedAdded?.Invoke(owned);
        }

        protected virtual void BeforeAddOwned(Node node)
        {
            if (!node.IsDirty) CurrentMoney += Rules.MoneyForFirstCapturingNode;
            Income += Mathf.RoundToInt(Rules.IncomeModifier.Modify(node.BaseIncome));
            Score += Rules.ScoreForCapturingNodeModifier.Modify(node.Score);
        }
        
        protected virtual void BeforeAddOwned(Unit unit)
        {
            
        }

        public void RemoveOwned([NotNull] Owned owned)
        {
            if (owned == null) throw new ArgumentNullException(nameof(owned));

            if (!myOwned.Contains(owned)) return;

            switch (owned)
            {
                case Node node:
                    BeforeRemoveOwned(node);
                    break;
                case Unit unit:
                    BeforeRemoveOwned(unit);
                    break;
            }
            
            myOwned.Remove(owned);
            OwnedRemoved?.Invoke(owned);
        }
        
        protected virtual void BeforeRemoveOwned(Node node)
        {
            Income -= Mathf.RoundToInt(Rules.IncomeModifier.Modify(node.BaseIncome));
            Score -= Rules.ScoreForCapturingNodeModifier.Modify(node.Score);
        }

        protected virtual void BeforeRemoveOwned(Unit unit)
        {
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