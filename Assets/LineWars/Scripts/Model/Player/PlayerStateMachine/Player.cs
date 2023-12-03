using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using LineWars.Interface;
using LineWars.Model;
using UnityEngine;
using LineWars.Controllers;

namespace LineWars
{
    public partial class Player : BasePlayer
    {
        public static Player LocalPlayer { get; private set; }
        [SerializeField] private float pauseAfterTurn;

        private IReadOnlyCollection<UnitType> potentialExecutors;
        private readonly HashSet<Node> additionalVisibleNodes = new();

        public event Action VisibilityRecalculated;

        public PhaseType? CurrentPhase { get; protected set; } 
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

        private void UnitOnDied(Unit diedUnit)
        {
            RecalculateVisibility();
            if (CurrentPhase != null 
                && (object)CommandsManager.Instance.Executor != diedUnit) //Из-за этого случался баг с ShotUnit?
            {
                FinishTurn();
            }
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

        public void FinishTurn() //Сделать InvokeTurnEnded virtual?
        {
            if(CurrentPhase == null)
            {
                Debug.LogError("Cannot finish turn: CurrentPhase == null!");
                return;
            }
            InvokeTurnEnded(CurrentPhase.Value);
            CurrentPhase = null;
        }

        public override void ExecuteTurn(PhaseType phaseType)
        {
            CurrentPhase = phaseType;
            InvokeTurnStarted(phaseType);
            switch (phaseType)
            {
                case PhaseType.Buy:
                    GameUI.Instance.SetEnemyTurn(false);
                    break;
                case PhaseType.Replenish:
                    ExecuteReplenish();
                    break;
                default:
                    potentialExecutors = PhaseExecutorsData[phaseType];
                    GameUI.Instance.SetEnemyTurn(false);
                    GameUI.Instance.ReDrawAllAvailability(GetAllUnitsByPhase(phaseType), true);
                    GameUI.Instance.SetEnemyTurn(false);
                    break;
            }
        }

        protected override void ExecuteReplenish()
        {
            base.ExecuteReplenish();
            if (additionalVisibleNodes.Count > 0)
            {
                additionalVisibleNodes.Clear();
                RecalculateVisibility();
            }
            FinishTurn();
        }

        public override bool CanExecuteTurn(PhaseType phaseType)
        {
            if(!base.CanExecuteTurn(phaseType)) //плохо? сделать явное сравнение на PhaseExceptions?
                return false;
            if (phaseType == PhaseType.Buy || phaseType == PhaseType.Replenish)
                return true;

            var phaseExecutors = PhaseExecutorsData.PhaseToUnits[phaseType];

            foreach (var owned in OwnedObjects)
            {
                if(!(owned is Unit unit)) continue;
                if(phaseExecutors.Contains(unit.Type) && unit.CurrentActionPoints > 0)
                    return true;
            }

            return false;
        }
        
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