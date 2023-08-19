using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public partial class EnemyAI : BasePlayer
    {
        [SerializeField] private PhaseExecutorsData executorsData;
        [SerializeField] private EnemyPhaseActions enemyPhaseActions;
        [SerializeField] private EnemyDifficulty difficulty;

        public IReadOnlyCollection<UnitType> PotentialExecutors => executorsData.PhaseToUnits[CurrentPhase];
        
        #region Turns
        public override void ExecuteBuy()
        {
            
        }

        public override void ExecuteArtillery()
        {
        }

        public override void ExecuteFight()
        {
        }

        public override void ExecuteScout()
        {
        }


        public override void ExecuteIdle()
        {
            
        }

        public override void ExecuteReplenish()
        {
        }

        private void ExecutePhase(PhaseType phase)
        {
            IExecutor executor;
            var actions = new List<EnemyAction>();
            var possibleActionData = enemyPhaseActions.PhasesToActions[phase];
            var unitTypes = executorsData.PhaseToUnits[phase];
            foreach (var owned in OwnedObjects) 
                if (owned is Unit unit && unitTypes.Contains(unit.Type))
                {
                    foreach (var actionData in possibleActionData)
                    {
                        actionData.AddAllPossibleActions(actions, this, unit);
                    }
                }
            
            actions.Sort();
            
            
            
    


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

