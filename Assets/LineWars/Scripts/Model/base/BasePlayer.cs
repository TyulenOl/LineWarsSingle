using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;


namespace LineWars.Model
{
    /// <summary>
    /// класс, содержащий всю логику, которая объединяет ИИ и игрока
    /// </summary>
    public abstract class BasePlayer : MonoBehaviour, IActor, IBasePlayer
    {
        [field: SerializeField, ReadOnlyInspector] public int Id { get; private set; }

        [SerializeField, ReadOnlyInspector] private int money;

        /// <summary>
        /// Для оптимизации income всегда хешируется
        /// </summary>
        [SerializeField, ReadOnlyInspector] private int income;

        [field: SerializeField] public PhaseExecutorsData PhaseExecutorsData { get; private set; }
        public NationEconomicLogic EconomicLogic => Nation.NationEconomicLogic;
        public List<Node> InitialSpawns { get; private set; }

        [field: SerializeField, ReadOnlyInspector] public Node Base { get; private set; }

        [field: SerializeField, ReadOnlyInspector] public PlayerRules Rules { get; private set; }

        public PhaseType CurrentPhase { get; private set; }
        public Nation Nation { get; private set; }

        public HashSet<PhaseType> PhaseExceptions { get; set; }


        private readonly HashSet<Owned> myOwned = new();
        private readonly List<Node> nodes = new();
        private readonly List<Unit> units = new();

        private bool isFirstReplenish = true;

        public IEnumerable<Node> MyNodes => nodes;
        public IEnumerable<Unit> MyUnits => units;

        public event Action<PhaseType, PhaseType> TurnChanged;
        public event Action<Owned> OwnedAdded;
        public event Action<Owned> OwnedRemoved;
        public event Action<int, int> CurrentMoneyChanged;
        public event Action<int, int> IncomeChanged;
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

        protected virtual void Awake()
        {
            PhaseExceptions = new HashSet<PhaseType>();
        }

        protected virtual void Start()
        {
            PhaseManager.Instance.RegisterActor(this);
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        public virtual void Initialize(SpawnInfo spawnInfo)
        {
            Id = spawnInfo.PlayerIndex;
            Base = spawnInfo.SpawnNode.Node;
            Rules = spawnInfo.SpawnNode.Rules ? spawnInfo.SpawnNode.Rules : PlayerRules.DefaultRules;

            CurrentMoney = Rules.StartMoney;
            Income = Rules.DefaultIncome;
            Nation = spawnInfo.SpawnNode.Nation;
            
            name = $"{GetType().Name}{spawnInfo.PlayerIndex} {spawnInfo.SpawnNode.name}";

            InitialSpawns = spawnInfo.SpawnNode.InitialSpawns;
        }
        
        #region SpawnUnit

        public bool CanSpawnUnit(Node node, UnitType type)
        {
            var unitPrefab = GetUnitPrefab(type);
            return CanSpawnUnit(node, unitPrefab);
        }

        public bool CanSpawnUnit(Node node, Unit unit)
        {
            if (unit.Size == UnitSize.Large)
                return node.AllIsFree;
            return node.AnyIsFree;
        }

        public Unit SpawnUnit(Node node, UnitType unitType)
        {
            if (unitType == UnitType.None) return null;
            var unitPrefab = GetUnitPrefab(unitType);
            return SpawnUnit(node, unitPrefab);
        }

        public Unit SpawnUnit(Node node, Unit unitPrefab)
        {
            var unitInstance =BasePlayerUtility.CreateUnitForPlayer(this, node, unitPrefab);
            OnSpawnUnit();
            return unitInstance;
        }

        protected virtual void OnSpawnUnit()
        {
        }

        #endregion

        #region BuyPreset

        public bool CanBuyPreset(UnitBuyPreset preset)
        {
            var nodes = MonoGraph.Instance.Nodes.Where(x => x.Owner == this);
            var c = nodes.Select(x => CanBuyPreset(preset, x));
            var b = c.Any(x => x);

            return b;
        }

        public bool CanBuyPreset(UnitBuyPreset preset, Node node)
        {
            if (preset.FirstUnitType != UnitType.None && preset.SecondUnitType == UnitType.None)
            {
                return CanAffordPreset(preset)
                    && CanSpawnUnit(node, preset.FirstUnitType);
            }
            if (preset.FirstUnitType == UnitType.None && preset.SecondUnitType != UnitType.None)
            {
                return CanAffordPreset(preset)
                    && CanSpawnUnit(node, preset.SecondUnitType);
            }

            if (preset.FirstUnitType != UnitType.None && preset.SecondUnitType != UnitType.None)
                return CanBuyPresetMultiple(preset, node);
            Debug.Log("Invalid preset!");
            return false;
        }
        
        private bool CanBuyPresetMultiple(UnitBuyPreset preset, Node node)
        {
            if (GetUnitPrefab(preset.FirstUnitType).Size == UnitSize.Large
                || GetUnitPrefab(preset.SecondUnitType).Size == UnitSize.Large)
                Debug.LogError("Invalid Preset!");
            return CanAffordPreset(preset)
                   && (node.AllIsFree);
        }

        private bool CanAffordPreset(UnitBuyPreset preset)
        {
            var purchaseInfo = this.GetPresetPurchaseInfo(preset);
            return purchaseInfo.CanBuy && CurrentMoney - purchaseInfo.Cost >= 0;
        }

        public void BuyPreset(UnitBuyPreset unitPreset)
        {
            BuyPreset(unitPreset, Base);
        }

        public void BuyPreset(UnitBuyPreset preset, Node node)
        {
            CurrentMoney -= this.GetPresetPurchaseInfo(preset).Cost;
            var unitsList = new List<Unit>
            {
                SpawnUnit(node, preset.FirstUnitType),
                SpawnUnit(node, preset.SecondUnitType)
            }.Where(x => x != null);
            OnBuyPreset(node, unitsList);
        }

        protected virtual void OnBuyPreset(Node node, IEnumerable<Unit> units) { }

        #endregion

        public void AddOwned([NotNull] Owned owned)
        {
            if (owned == null) throw new ArgumentNullException(nameof(owned));

            if (owned.Owner != null)
            {
                throw new InvalidOperationException();
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
            nodes.Add(node);
            var nodeIncome = GetMyIncomeFromNode(node);
            if (!node.IsDirty) CurrentMoney += GetMyCapturingMoneyFromNode(node);
            Income += nodeIncome;
        }

        public int GetMyIncomeFromNode(Node node)
        {
            return Mathf.RoundToInt(Rules.IncomeModifier.Modify(node.BaseIncome));
        }

        public int GetMyCapturingMoneyFromNode(Node node)
        {
            return Rules.MoneyForFirstCapturingNode;
        }

        protected virtual void BeforeAddOwned(Unit unit)
        {
            units.Add(unit);
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
            nodes.Remove(node);
            Income -= Mathf.RoundToInt(Rules.IncomeModifier.Modify(node.BaseIncome));
        }

        protected virtual void BeforeRemoveOwned(Unit unit)
        {
            units.Remove(unit);
        }
        
        public Unit GetUnitPrefab(UnitType unitType) => Nation.GetUnitPrefab(unitType);

        public void FinishTurn()
        {
            StartCoroutine(Coroutine());

            IEnumerator Coroutine()
            {
                yield return null;
                ExecuteTurn(PhaseType.Idle);
            }
        }

        public void ExecuteTurn(PhaseType phaseType)
        {
            if (PhaseExceptions.Contains(phaseType))
            {
                StartCoroutine(SkipTurnCoroutine());
                return;
            }

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

            IEnumerator SkipTurnCoroutine()
            {
                yield return null;
                TurnChanged?.Invoke(phaseType, PhaseType.Idle);
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
                case PhaseType.Replenish:
                    return CanExecuteReplenish();
            }

            Debug.LogWarning
            ($"Phase.{phaseType} is not implemented in \"CanExecuteTurn\"! "
             + "Change IActor to acommodate for this phase!");
            return false;
        }

        #region Turns

        protected virtual void ExecuteBuy()
        {
        }

        protected virtual void ExecuteArtillery()
        {
        }

        protected virtual void ExecuteFight()
        {
        }

        protected virtual void ExecuteScout()
        {
        }


        protected virtual void ExecuteIdle()
        {
        }

        protected virtual void ExecuteReplenish()
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

        protected virtual bool CanExecuteBuy()
        {
            return false;
        }

        protected virtual bool CanExecuteArtillery()
        {
            return false;
        }

        protected virtual bool CanExecuteFight()
        {
            return false;
        }

        protected virtual bool CanExecuteScout()
        {
            return false;
        }

        protected virtual bool CanExecuteReplenish()
        {
            return false;
        }

        #endregion
    }
}