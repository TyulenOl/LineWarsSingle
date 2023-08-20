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

        public IReadOnlyCollection<UnitType> PotentialExecutors => executorsData.PhaseToUnits[CurrentPhase];
        
        #region Turns
        public override void ExecuteBuy() => ExecutePhase(PhaseType.Buy);
        public override void ExecuteArtillery() => ExecutePhase(PhaseType.Artillery);
        public override void ExecuteFight() => ExecutePhase(PhaseType.Fight);
        public override void ExecuteScout() => ExecutePhase(PhaseType.Scout);
        private void ExecutePhase(PhaseType phase)
        {
            StartCoroutine(ExecuteCoroutine());
            

            IEnumerator ExecuteCoroutine()
            {
                var actions = new List<EnemyAction>();
                foreach (var owned in OwnedObjects)
                {
                    if (!(owned is IExecutor executor)) continue;
                
                    AddActionsForExecutor(actions, executor, phase);
                }

                var chosenAction = PickAction(actions);
                var chosenExecutor = chosenAction.Executor;
                chosenAction.Execute();
                yield return new WaitForSeconds(actionCooldown);
                
                while (chosenExecutor.CurrentActionPoints > 0)
                {
                    var chosenExecutorsActions = new List<EnemyAction>();
                    AddActionsForExecutor(chosenExecutorsActions, chosenExecutor, phase);
                    var newAction = PickAction(chosenExecutorsActions);
                    newAction.Execute();
                    yield return new WaitForSeconds(actionCooldown);
                }
                
                ExecutePhase(PhaseType.Idle);
            }
        }

        private void AddActionsForExecutor(List<EnemyAction> actions, IExecutor executor, PhaseType phase)
        {
            var unitTypes = executorsData.PhaseToUnits[phase];
            if (executor is Unit unit && !unitTypes.Contains(unit.Type)) return;
            
            var possibleActionData = enemyPhaseActions.PhasesToActions[phase];
            foreach (var actionData in possibleActionData)
            {
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
                randomList.Add(currentAction, 
                    currentAction.Score * difficulty.Curve.Evaluate((float) i/ sortedList.Count));
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
                if (!(owned is Unit unit)) continue;
                if(executors.Contains(unit.Type) && unit.CurrentActionPoints > 0)
                    return true;
            }

            return false;
        }
        #endregion
    }
}

