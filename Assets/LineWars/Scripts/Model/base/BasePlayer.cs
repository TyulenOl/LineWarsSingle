using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;


namespace LineWars.Model
{
    /// <summary>
    /// класс, содержащий всю логику, которая объединяет ИИ и игрока
    /// </summary>
    public abstract class BasePlayer : MonoBehaviour, IActor, IBasePlayer
    {
        [field: SerializeField] public int Id { get; private set; } = -1;

        [field: Header("Settings")]
        [field: SerializeField] public PhaseExecutorsData PhaseExecutorsData { get; private set; }

        [field: SerializeField] public PlayerRules Rules { get; private set; }
        [field: SerializeField] public Nation Nation { get; private set; }

        [SerializeField, ReadOnlyInspector] private int currentMoney;
        [SerializeField, ReadOnlyInspector] private int currentIncome;

        [field: SerializeField] public List<Node> InitialSpawns { get; set; }
        [field: SerializeField] public List<Node> InitialNodes { get; set; }
        public IEnumerable<Node> AllInitialNodes => InitialSpawns.Concat(InitialNodes);
        
        
        public Node Base { get; private set; }
        public HashSet<PhaseType> PhaseExceptions { get; private set; }

        private readonly HashSet<Owned> myOwned = new();
        private readonly HashSet<Node> nodes = new();

        private readonly HashSet<Unit> units = new();

        public IEnumerable<Node> MyNodes => nodes;
        public IEnumerable<Unit> MyUnits => units;

        public event Action<Owned> OwnedAdded;
        public event Action<Owned> OwnedRemoved;
        public event Action<int, int> CurrentMoneyChanged;
        public event Action<int, int> IncomeChanged;
        public event Action<IActor, PhaseType> TurnStarted;
        public event Action<IActor, PhaseType> TurnEnded;

        public IReadOnlyCollection<Owned> OwnedObjects => myOwned;
        public bool IsMyOwn(Owned owned) => myOwned.Contains(owned);

        public int CurrentMoney
        {
            get => currentMoney;
            set
            {
                var before = currentMoney;
                currentMoney = Math.Max(0, value);

                if (before != currentMoney)
                    CurrentMoneyChanged?.Invoke(before, currentMoney);
            }
        }

        public int Income
        {
            get => currentIncome;
            set
            {
                var before = currentIncome;
                currentIncome = value;
                IncomeChanged?.Invoke(before, currentIncome);
            }
        }

        protected virtual void Awake()
        {
            PhaseExceptions = new HashSet<PhaseType>();
        }

        protected virtual void Start()
        {
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        public void Initialize(int id)
        {
            Id = id;
            Base = InitialSpawns.First();

            CurrentMoney = Rules.StartMoney;
            Income = Rules.DefaultIncome;

            name = $"{GetType().Name}{id}";
            
            var initialSpawnsSet = InitialSpawns.ToHashSet();
            InitialNodes = InitialNodes
                .Where(x => !initialSpawnsSet.Contains(x))
                .ToList();
            
            OnInitialized();
        }

        protected virtual void OnInitialized()
        {
        }

        #region SpawnUnit

        public bool CanSpawnUnit(Node node, UnitType type)
        {
            var unitPrefab = GetUnitPrefab(type);
            return CanSpawnUnit(node, unitPrefab);
        }

        public bool CanSpawnUnit(Node node, Unit unit)
        {
            return unit.Size == UnitSize.Large ? node.AllIsFree : node.AnyIsFree;
        }

        public Unit SpawnUnit(Node node, UnitType unitType)
        {
            if (unitType == UnitType.None) return null;
            var unitPrefab = GetUnitPrefab(unitType);
            return SpawnUnit(node, unitPrefab);
        }

        public Unit SpawnUnit(Node node, Unit unitPrefab)
        {
            var unitInstance = BasePlayerUtility.CreateUnitForPlayer(this, node, unitPrefab);
            OnSpawnUnit();
            return unitInstance;
        }

        protected virtual void OnSpawnUnit()
        {
        }

        #endregion

        #region BuyDeckCard

        public bool CanBuyDeckCard(DeckCard deckCard)
        {
            return MyNodes.Any(node => CanBuyDeckCard(node, deckCard));
        }

        public bool CanBuyDeckCard(Node node, DeckCard deckCard)
        {
            var cost = GetDeckCardPurchaseInfo(deckCard);
            return CanAfford(cost) && CanSpawnUnit(node, deckCard.Unit);
        }

        public void BuyDeckCard(Node node, DeckCard deckCard)
        {
            CurrentMoney -= GetDeckCardPurchaseInfo(deckCard);
            SpawnUnit(node, deckCard.Unit);
        }

        #endregion

        public bool CanAfford(PurchaseInfo purchaseInfo)
        {
            return purchaseInfo.CanBuy && CurrentMoney - purchaseInfo.Cost >= 0;
        }

        public void AddOwned([NotNull] Owned owned)
        {
            if (owned == null) throw new ArgumentNullException(nameof(owned));

            if (owned.Owner != null)
            {
                throw new InvalidOperationException();
            }

            if (myOwned.Contains(owned))
                throw new InvalidOperationException();

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

        public virtual bool CanExecuteTurn(PhaseType phaseType)
        {
            return !PhaseExceptions.Contains(phaseType);
        }

        public abstract void ExecuteTurn(PhaseType phaseType);

        protected virtual void ExecuteReplenish()
        {
            CurrentMoney += Income;
            foreach (var owned in OwnedObjects)
                owned.Replenish();
        }

        protected void InvokeTurnStarted(PhaseType phaseType)
        {
            TurnStarted?.Invoke(this, phaseType);
        }

        protected void InvokeTurnEnded(PhaseType phaseType)
        {
            TurnEnded?.Invoke(this, phaseType);
        }

        public int GetCountUnitByType(UnitType type)
        {
            return MyUnits.Count(x => x.Type == type);
        }

        public PurchaseInfo GetDeckCardPurchaseInfo(DeckCard deckCard)
        {
            return deckCard.CalculateCost(GetCountUnitByType(deckCard.Unit.Type));
        }

        public PurchaseInfo GetPurchaseInfoForUnit(UnitType unitType, int cost)
        {
            return Rules.CostFunction.Calculate(
                unitType,
                cost,
                GetCountUnitByType(unitType));
        }


        public IEnumerable<Unit> GetAllEnemiesUnits()
        {
            return SingleGameRoot.Instance.AllUnits.Values
                .Where(x => x.OwnerId != Id);
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            if (EditorExtensions.CanRedraw(gameObject))
            {
                UnityEditor.EditorApplication.delayCall += RedrawAllPlayers;
            }
        }
#endif
        public static void RedrawAllPlayers()
        {
            foreach (var node in FindObjectsOfType<Node>())
                node.DrawToDefault();

            foreach (var player in FindObjectsOfType<BasePlayer>())
            {
                if (player.Nation == null)
                    continue;
                foreach (var spawn in player.InitialSpawns.Where(x => x != null))
                    spawn.Redraw(true, player.Nation.Name, player.Nation.NodeSprite);
                foreach (var node in player.InitialNodes.Where(x => x != null))
                    node.Redraw(false, player.Nation.Name, player.Nation.NodeSprite);
            }
        }
    }
}