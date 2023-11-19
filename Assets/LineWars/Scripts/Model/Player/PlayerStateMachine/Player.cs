using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Interface;
using LineWars.Model;
using UnityEngine;
using UnityEngine.Events;

namespace LineWars
{
    public partial class Player : BasePlayer
    {
        public static Player LocalPlayer { get; private set; }
        [SerializeField] private float pauseAfterTurn;

        private IReadOnlyCollection<UnitType> potentialExecutors;
        private bool isTurnMade;
        private readonly HashSet<Node> additionalVisibleNodes = new ();

        private StateMachine stateMachine;
        private PlayerPhase idlePhase;
        private PlayerPhase artilleryPhase;
        private PlayerPhase fightPhase;
        private PlayerPhase scoutPhase;
        private PlayerBuyPhase buyPhase;
        private PlayerReplenishPhase replenishPhase;

        public event Action VisibilityRecalculated;
        
        public IReadOnlyCollection<UnitType> PotentialExecutors => potentialExecutors;
        public IReadOnlyDictionary<Node, bool> VisibilityMap { get; private set; }
        public IEnumerable<Node> AdditionalVisibleNodes => additionalVisibleNodes;


        protected override void Awake()
        {
            base.Awake();
            if (LocalPlayer != null)
                Debug.LogError("More than two players on the scene!");
            else
                LocalPlayer = this;

            
            InitializeStateMachine();
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            OwnedAdded += OnOwnedAdded;
            OwnedRemoved += OnOwnerRemoved;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            OwnedAdded -= OnOwnedAdded;
            OwnedRemoved -= OnOwnerRemoved;
        }

        private void InitializeStateMachine()
        {
            stateMachine = new StateMachine();
            idlePhase = new PlayerPhase(this, PhaseType.Idle);
            buyPhase = new PlayerBuyPhase(this, PhaseType.Buy);
            artilleryPhase = new PlayerPhase(this, PhaseType.Artillery);
            fightPhase = new PlayerPhase(this, PhaseType.Fight);
            scoutPhase = new PlayerPhase(this, PhaseType.Scout);
            replenishPhase = new PlayerReplenishPhase(this, PhaseType.Replenish);
        }

        private void OnOwnedAdded(Owned owned)
        {
            if(!(owned is Unit unit)) return;
            unit.Died.AddListener(UnitOnDied);
        }

        private void OnOwnerRemoved(Owned owned)
        {
            if(!(owned is Unit unit)) return;
            if (!unit.IsDied)
                unit.Died.RemoveListener(UnitOnDied);
        }
        public IEnumerable<Unit> GetAllUnitsByPhase(PhaseType phaseType)
        {
            if (PhaseExecutorsData.PhaseToUnits.TryGetValue(phaseType, out var value))
            {
                foreach (var myUnit in MyUnits)
                {
                    if (value.Contains(myUnit.Type))
                    {
                        yield return myUnit;
                    }
                }
            }
        }
        
        #region Turns

        protected override void ExecuteBuy()
        {
            stateMachine.SetState(buyPhase);
            GameUI.Instance.SetEnemyTurn(false);
        }

        protected override void ExecuteArtillery()
        {
            stateMachine.SetState(artilleryPhase);
            GameUI.Instance.ReDrawAllAvailability(GetAllUnitsByPhase(PhaseType.Artillery), true);
            GameUI.Instance.SetEnemyTurn(false);
        }

        protected override void ExecuteFight()
        {
            stateMachine.SetState(fightPhase);
            GameUI.Instance.ReDrawAllAvailability(GetAllUnitsByPhase(PhaseType.Fight), true);
            GameUI.Instance.SetEnemyTurn(false);
        }

        protected override void ExecuteScout()
        {
            stateMachine.SetState(scoutPhase);
            GameUI.Instance.ReDrawAllAvailability(GetAllUnitsByPhase(PhaseType.Scout), true);
            GameUI.Instance.SetEnemyTurn(false);
        }

        protected override void ExecuteIdle()
        {
            GameUI.Instance.ReDrawAllAvailability(MyUnits, false);
            GameUI.Instance.SetEnemyTurn(true);
            stateMachine.SetState(idlePhase);
        }

        protected override void ExecuteReplenish()
        {
            base.ExecuteReplenish();
            stateMachine.SetState(replenishPhase);
            if (additionalVisibleNodes.Count > 0)
            {
                additionalVisibleNodes.Clear();
                RecalculateVisibility();
            }
        }

        #endregion

        #region Check Turns

        protected override bool CanExecuteBuy() => true;

        protected override bool CanExecuteArtillery() => CanExecutePhase(PhaseType.Artillery);

        protected override bool CanExecuteFight() => CanExecutePhase(PhaseType.Fight);

        protected override bool CanExecuteScout() => CanExecutePhase(PhaseType.Scout);

        protected override bool CanExecuteReplenish() => true;

        private bool CanExecutePhase(PhaseType phaseType)
        {
            var phaseExecutors = PhaseExecutorsData.PhaseToUnits[phaseType];

            foreach (var owned in OwnedObjects)
            {
                if(!(owned is Unit unit)) continue;
                if(phaseExecutors.Contains(unit.Type) && unit.CurrentActionPoints > 0)
                    return true;
            }

            return false;
        }

        #endregion

        
        public void AddAdditionalVisibleNode(Node node)
        {
            if (!MonoGraph.Instance.Nodes.Contains(node))
                throw new ArgumentException();
            if (node == null)
                throw new ArgumentException();
            if (additionalVisibleNodes.Contains(node))
                return;
            additionalVisibleNodes.Add(node);
        }
        public bool RemoveVisibleNode(Node node) => additionalVisibleNodes.Remove(node);
        
        private void UnitOnDied(Unit diedUnit)
        {
            StartCoroutine(UnitOnDiedCoroutine());
            IEnumerator UnitOnDiedCoroutine()
            {
                yield return null;
                RecalculateVisibility();
                if(CurrentPhase != PhaseType.Idle)
                {
                    ExecuteTurn(PhaseType.Idle);
                }
            }
        }

        protected override void OnBuyPreset(Node node, IEnumerable<Unit> units)
        {
            base.OnBuyPreset(node, units);
            RecalculateVisibility();
        }

        public void RecalculateVisibility(bool useLerp = true)
        {
            var visibilityMap = MonoGraph.Instance.GetVisibilityInfo(this);
            foreach (var node in additionalVisibleNodes)
                visibilityMap[node] = true;
            
            foreach (var (node, visibility) in visibilityMap)
            {
                if (useLerp)
                    node.RenderNodeV3.SetVisibilityGradually(visibility ? 1 : 0);
                else
                    node.RenderNodeV3.SetVisibilityInstantly(visibility ? 1 : 0);
            }

            VisibilityMap = visibilityMap;
            VisibilityRecalculated?.Invoke();
        }
    }
}