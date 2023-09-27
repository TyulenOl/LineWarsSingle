using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataStructures;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public partial class EnemyAI : BasePlayer
    {
        [SerializeField] private PhaseExecutorsData executorsData;
        [SerializeField] private EnemyPhaseActions enemyPhaseActions;
        [SerializeField] private EnemyDifficulty difficulty;
        [SerializeField] private float actionCooldown;
        [SerializeField] private EnemyAIPersonality personality;

        private IReadOnlyExecutor currentExecutor;
        private EnemyAIBuySelector buySelector;
        public IReadOnlyCollection<UnitType> PotentialExecutors => executorsData.PhaseToUnits[CurrentPhase];

        protected override void Awake()
        {
            base.Awake();
            buySelector = new EnemyAIBuySelector();
        }

        #region Turns

        public override void ExecuteBuy()
        {
            StartCoroutine(BuyCoroutine());
            IEnumerator BuyCoroutine()
            {
                if(buySelector.TryGetPreset(this, out var preset))
                    SpawnPreset(preset);
                yield return null;
                ExecuteTurn(PhaseType.Idle);
            }
        }
        public override void ExecuteArtillery() => ExecuteAITurn(PhaseType.Artillery);
        public override void ExecuteFight() => ExecuteAITurn(PhaseType.Fight);
        public override void ExecuteScout() => ExecuteAITurn(PhaseType.Scout);

        public override void ExecuteReplenish()
        {
            base.ExecuteReplenish();
            StartCoroutine(ReplenishCoroutine());
            IEnumerator ReplenishCoroutine()
            {
                yield return null;
                ExecuteTurn(PhaseType.Idle);
            }
        }
        
        private void ExecuteAITurn(PhaseType phase)
        {
            var actions = new List<EnemyAction>();
            foreach (var owned in OwnedObjects)
            {
                
                if (!(owned is IReadOnlyExecutor executor)) continue;
                if(executor.CurrentActionPoints <= 0) continue;
                AddActionsForExecutor(actions, executor, phase);
            }

            var chosenAction = PickAction(actions);
            currentExecutor = chosenAction.Executor;
            chosenAction.ActionCompleted += GetActionOnActionCompleted(phase);
            Debug.Log($"{chosenAction} CHOSEN");
            chosenAction.Execute();
        }
        
        private Action<EnemyAction> GetActionOnActionCompleted(PhaseType phase)
        {
            return (action) => OnActionCompleted(action, phase);
        }

        private void OnActionCompleted(EnemyAction previousAction, PhaseType phaseType)
        {
            previousAction.ActionCompleted -= GetActionOnActionCompleted(phaseType);
            StartCoroutine(NewActionCoroutine());
            IEnumerator NewActionCoroutine()
            {
                yield return new WaitForSeconds(actionCooldown);
                if (currentExecutor.CurrentActionPoints > 0)
                {
                    yield return new WaitForSeconds(actionCooldown);
                    var actions = new List<EnemyAction>();
                    AddActionsForExecutor(actions, currentExecutor, phaseType);
                    var newAction = PickAction(actions);
                    newAction.ActionCompleted += GetActionOnActionCompleted(phaseType);
                    newAction.Execute();

                }
                else
                {
                    currentExecutor = null;
                    ExecuteTurn(PhaseType.Idle);
                }
            }
        }

        private void AddActionsForExecutor(List<EnemyAction> actions, IReadOnlyExecutor executor, PhaseType phase)
        {
            var unitTypes = executorsData.PhaseToUnits[phase];
            if (executor is ComponentUnit unit && !unitTypes.Contains(unit.Type)) return;
            
            var possibleActionData = enemyPhaseActions.PhasesToActions[phase];
            foreach (var actionData in possibleActionData)
            {
                Debug.Log(actionData);
                actionData.AddAllPossibleActions(actions, this, executor);
            }
        }

        private EnemyAction PickAction(List<EnemyAction> actions)
        {
            var sortedList = new List<EnemyAction>(actions);
            sortedList.Sort();
            var randomList = new RandomChanceList<EnemyAction>();
            for (var i = 0; i < sortedList.Count; i++)
            {
                var currentAction = sortedList[i];
                Debug.Log($"action - {currentAction}, score - {currentAction.Score}, i - {i}, count - {sortedList.Count}, " +
                          $"time - {((float) i + 1)/ sortedList.Count}, evaluate - {difficulty.Curve.Evaluate(((float) i + 1)/ sortedList.Count)}");
                randomList.Add(currentAction, 
                    difficulty.Curve.Evaluate(((float) i + 1)/ sortedList.Count));
            }

            return randomList.PickRandomObject();
        }

        #endregion

        #region Check Turns
        public override bool CanExecuteBuy() => true;
        public override bool CanExecuteArtillery() => CanExecutePhase(PhaseType.Artillery);
        public override bool CanExecuteFight() => CanExecutePhase(PhaseType.Fight);
        public override bool CanExecuteScout() => CanExecutePhase(PhaseType.Scout);
        public override bool CanExecuteReplenish() => true;

        private bool CanExecutePhase(PhaseType phase)
        {
            var executors = executorsData.PhaseToUnits[phase];
            foreach (var owned in OwnedObjects)
            {
                if (!(owned is ComponentUnit unit)) continue;
                if(executors.Contains(unit.Type) && unit.CurrentActionPoints > 0)
                    return true;
            }

            return false;
        }
        #endregion
    }
}

