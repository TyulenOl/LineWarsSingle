using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using LineWars.Interface;
using LineWars.Model;
using UnityEngine;
using LineWars.Controllers;

namespace LineWars.Model
{
    public class Player : BasePlayer
    {
        public static Player LocalPlayer { get; private set; }

        //private IReadOnlyCollection<UnitType> potentialExecutors;
        private readonly Dictionary<Node, int> additionalVisibleNodesRound = new();
        private readonly Dictionary<Node, int> additionalVisibleNodeTurn = new();

        public event Action VisibilityRecalculated;

        public PhaseType? CurrentPhase { get; protected set; } 
        //public IReadOnlyCollection<UnitType> PotentialExecutors => potentialExecutors;
        public IReadOnlyDictionary<Node, bool> VisibilityMap { get; private set; }


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
            // if (CurrentPhase != null 
            //     && (object)CommandsManager.Instance.Executor != diedUnit) //Из-за этого случался баг с ShotUnit? Возможно
            // {
            //     FinishTurn();
            // }
        }

        // public IEnumerable<Unit> GetAllUnitsByPhase(PhaseType phaseType)
        // {
        //     if (PhaseExecutorsData.PhaseToUnits.TryGetValue(phaseType, out var value))
        //     {
        //         foreach (var myUnit in MyUnits)
        //         {
        //             if (value.Contains(myUnit.Type))
        //             {
        //                 yield return myUnit;
        //             }
        //         }
        //     }
        // }

        public void FinishTurn() 
        {
            if(CurrentPhase == null)
            {
                Debug.LogError("Cannot finish turn: CurrentPhase == null!");
                return;
            }

            if (additionalVisibleNodeTurn.Count > 0)
            {
                foreach (var node in additionalVisibleNodeTurn.Keys.ToArray())
                {
                    additionalVisibleNodeTurn[node]--;
                    if (additionalVisibleNodeTurn[node] <= 0)
                        additionalVisibleNodeTurn.Remove(node);
                }
                RecalculateVisibility();
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
                    GameUI.Instance.SetEconomyTurn();
                    break;
                case PhaseType.Replenish:
                    ExecuteReplenish();
                    break;
                default:
                    //potentialExecutors = PhaseExecutorsData[phaseType];
                    GameUI.Instance.SetEnemyTurn(false);
                    GameUI.Instance.ReDrawAllAvailability(MyUnits, true);
                    break;
            }
        }

        protected override void ExecuteReplenish()
        {
            base.ExecuteReplenish();
            if (additionalVisibleNodesRound.Count > 0)
            {
                foreach (var node in additionalVisibleNodesRound.Keys.ToArray())
                {
                    additionalVisibleNodesRound[node]--;
                    if (additionalVisibleNodesRound[node] <= 0)
                        additionalVisibleNodesRound.Remove(node);
                }
                
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

            //var phaseExecutors = PhaseExecutorsData.PhaseToUnits[phaseType];

            foreach (var owned in OwnedObjects)
            {
                if(!(owned is Unit unit)) continue;
                if(/*phaseExecutors.Contains(unit.Type) &&*/ unit.CurrentActionPoints > 0)
                    return true;
            }

            return false;
        }
        
        /// <summary>
        /// Добавляет ноды в пулл дополнительно видимых, счет идет по РАУНДАМ 
        /// </summary>
        public void SetAdditionalVisibleNodeForRound(Node node, int roundsCount)
        {
            if (!MonoGraph.Instance.Nodes.Contains(node))
                throw new ArgumentException();
            if (node == null)
                throw new ArgumentException();
            additionalVisibleNodesRound[node] = roundsCount;
        }
        
        /// <summary>
        /// Добавляет ноды в пулл дополнительно видимых, счет идет по ХОДАМ 
        /// </summary>
        public void SetAdditionalVisibleNodeForTurn(Node node, int turnsCount)
        {
            if (!MonoGraph.Instance.Nodes.Contains(node))
                throw new ArgumentException();
            if (node == null)
                throw new ArgumentException();
            additionalVisibleNodeTurn[node] = turnsCount;
        }

        protected override void OnSpawnUnit()
        {
            base.OnSpawnUnit();
            RecalculateVisibility();
        }

        public void RecalculateVisibility(bool useLerp = true)
        {
            var visibilityMap = MonoGraph.Instance.GetVisibilityInfo(this);
            foreach (var node in additionalVisibleNodesRound.Keys)
                visibilityMap[node] = true;
            foreach (var node in additionalVisibleNodeTurn.Keys)
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