using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using LineWars.Extensions.Attributes;
using UnityEngine;


namespace LineWars.Model
{
    /// <summary>
    /// класс, содержащий всю логику, которая объединяет ИИ и игрока
    /// </summary>
    public abstract class BasePlayer : MonoBehaviour, IBasePlayer, IActor
    {
        [field: SerializeField, ReadOnlyInspector] public int Index { get;  private set; }
        [SerializeField, ReadOnlyInspector] private int money;
        /// <summary>
        /// Для оптимизации income всегда хешируется
        /// </summary>
        [SerializeField, ReadOnlyInspector] private int income;
        

        [field: SerializeField, ReadOnlyInspector] public Node Base { get; private set; }
        INode IBasePlayer.Base => Base;
        IReadOnlyNode IReadOnlyBasePlayer.Base => Base;
        [field: SerializeField, ReadOnlyInspector] public PlayerRules Rules { get;  set; }

        public PhaseType CurrentPhase { get; private set; }
        public Nation Nation { get; private set; }

        private HashSet<Owned> myOwned;
        private bool isFirstReplenish = true;
        
        private IEnumerable<Node> MyNodes => myOwned.OfType<Node>();
        protected IEnumerable<ComponentUnit> MyUnits => myOwned.OfType<ComponentUnit>();
        

        public event Action<PhaseType, PhaseType> TurnChanged;
        public event Action<Owned> OwnedAdded;
        public event Action<Owned> OwnedRemoved;
        public event Action<int, int> CurrentMoneyChanged;
        public event Action<int, int> IncomeChanged;
        public event Action Defeated; 
        public IReadOnlyCollection<Owned> OwnedObjects => myOwned;
        IReadOnlyCollection<IOwned> IBasePlayer.OwnedObjects => myOwned;
        IReadOnlyCollection<IReadOnlyOwned> IReadOnlyBasePlayer.OwnedObjects => myOwned;
        
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
            Base = spawnInfo.SpawnNode.Node;
            Rules = spawnInfo.SpawnNode.Rules ? spawnInfo.SpawnNode.Rules : PlayerRules.DefaultRules;

            CurrentMoney = Rules.StartMoney;
            Income = Rules.DefaultIncome;
            Nation = spawnInfo.SpawnNode.Nation;
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

        public void SpawnUnit(Node node, UnitType unitType)
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
                case ComponentUnit unit:
                    BeforeAddOwned(unit);
                    break;
            }

            myOwned.Add(owned);
            OwnedAdded?.Invoke(owned);
        }
        void IBasePlayer.AddOwned(IOwned owned) => AddOwned((Owned) owned);

        protected virtual void BeforeAddOwned(Node node)
        {
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

        protected virtual void BeforeAddOwned(ComponentUnit unit)
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
                case ComponentUnit unit:
                    BeforeRemoveOwned(unit);
                    break;
            }
            
            myOwned.Remove(owned);
            OwnedRemoved?.Invoke(owned);
        }

        void IBasePlayer.RemoveOwned(IOwned owned) => RemoveOwned((Owned)owned);

        protected virtual void BeforeRemoveOwned(Node node)
        {
            Income -= Mathf.RoundToInt(Rules.IncomeModifier.Modify(node.BaseIncome));

            if (node == Base)
            {
                Defeat();
            }
        }

        protected virtual void BeforeRemoveOwned(ComponentUnit unit)
        {
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
                node.SetOwner(null);

            myOwned = new HashSet<Owned>();
            Destroy(gameObject);
        }
        
        public ComponentUnit GetUnitPrefab(UnitType unitType) => Nation.GetUnitPrefab(unitType);

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