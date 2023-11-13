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
        [field: SerializeField, ReadOnlyInspector] public int Id { get;  private set; }
        [SerializeField, ReadOnlyInspector] private int money;
        /// <summary>
        /// Для оптимизации income всегда хешируется
        /// </summary>
        [SerializeField, ReadOnlyInspector] private int income;

        [field:SerializeField] public PhaseExecutorsData PhaseExecutorsData { get; private set; }
        [field:SerializeField] public NationEconomicLogic EconomicLogic { get; private set; }
        [field: SerializeField, ReadOnlyInspector] public Node Base { get; private set; }
        [field: SerializeField, ReadOnlyInspector] public PlayerRules Rules { get; set; }

        public PhaseType CurrentPhase { get; private set; }
        public Nation Nation { get; private set; }
        

        private HashSet<Owned> myOwned = new();
        private readonly List<Node> nodes = new ();
        private readonly List<Unit> units = new ();
        
        private bool isFirstReplenish = true;

        public IEnumerable<Node> MyNodes => nodes;
        public IEnumerable<Unit> MyUnits => units;
        
        public event Action<PhaseType, PhaseType> TurnChanged;
        public event Action<Owned> OwnedAdded;
        public event Action<Owned> OwnedRemoved;
        public event Action<int, int> CurrentMoneyChanged;
        public event Action<int, int> IncomeChanged;
        public event Action Defeated; 
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
            Id = spawnInfo.PlayerIndex;
            Base = spawnInfo.SpawnNode.Node;
            Rules = spawnInfo.SpawnNode.Rules ? spawnInfo.SpawnNode.Rules : PlayerRules.DefaultRules;

            CurrentMoney = Rules.StartMoney;
            Income = Rules.DefaultIncome;
            Nation = spawnInfo.SpawnNode.Nation;
            
            SingleGame.Instance.AllPlayers.Add(spawnInfo.PlayerIndex, this);
            name = $"{GetType().Name}{spawnInfo.PlayerIndex} {spawnInfo.SpawnNode.name}";
        }

        protected virtual void OnDestroy()
        {
            SingleGame.Instance.AllPlayers.Remove(this);
        }

        public bool CanBuyPreset(UnitBuyPreset preset)
        {
            return CanBuyPreset(preset, Base);
        }

        public bool CanBuyPreset(UnitBuyPreset preset, Node node)
        {
            if (preset.FirstUnitType != UnitType.None && preset.SecondUnitType == UnitType.None)
                return CanBuyPresetOne(preset, node);
            if(preset.FirstUnitType != UnitType.None && preset.SecondUnitType != UnitType.None)
                return CanBuyPresetMultiple(preset, node);
            Debug.Log("Invalid preset!");
            return false;    
        }

        public bool CanBuyPresetOne(UnitBuyPreset preset, Node node)
        {
            if(GetUnitPrefab(preset.FirstUnitType).Size == UnitSize.Little)
                return CanAffordPreset(preset)
                    && (node.AnyIsFree);
            return CanAffordPreset(preset)
                 && (node.AllIsFree);
        }

        public bool CanBuyPresetMultiple(UnitBuyPreset preset, Node node)
        {
            if (GetUnitPrefab(preset.FirstUnitType).Size == UnitSize.Large
                || GetUnitPrefab(preset.SecondUnitType).Size == UnitSize.Large)
                Debug.LogError("Invalid Preset!");
            return CanAffordPreset(preset)
                && (node.AllIsFree);
        }

        public bool CanAffordPreset(UnitBuyPreset preset)
        {
            return CurrentMoney - preset.Cost >= 0;
        }

        public void SpawnUnit(Node node, UnitType unitType)
        {
            if (unitType == UnitType.None) return;
            var unitPrefab = GetUnitPrefab(unitType);
            BasePlayerUtility.CreateUnitForPlayer(this, node, unitPrefab);
            OnSpawnUnit();
        }
        
        protected virtual void OnSpawnUnit(){}

        public void BuyPreset(UnitBuyPreset unitPreset)
        {
            SpawnUnit(Base, unitPreset.FirstUnitType);
            SpawnUnit(Base, unitPreset.SecondUnitType);
            CurrentMoney -= unitPreset.Cost;
            OnSpawnPreset();
            BuyPreset(unitPreset, Base);
        }

        public void BuyPreset(UnitBuyPreset preset, Node node) //TODO: Закинуть в IBasePlayer???
        {
            SpawnUnit(node, preset.FirstUnitType);
            SpawnUnit(node, preset.SecondUnitType);
            CurrentMoney -= preset.Cost;
        }
        
        public virtual void OnSpawnPreset() {}

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
            return Rules.MoneyForFirstCapturingNode + GetMyIncomeFromNode(node);
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

            if (node == Base)
            {
                Defeat();
            }
        }

        protected virtual void BeforeRemoveOwned(Unit unit)
        {
            units.Remove(unit);
        }

        public void Defeat()
        {
            OnDefeat();
            Defeated?.Invoke();
        }
        protected virtual void OnDefeat()
        {
            foreach (var unit in MyUnits.ToList())
                Destroy(unit.gameObject);
            foreach (var node in MyNodes.ToList()) 
                node.Owner = null;

            myOwned = new HashSet<Owned>();
            Destroy(gameObject);
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