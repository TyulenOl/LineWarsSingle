
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [System.Serializable]
    public class ActionsForPhase
    {
        [SerializeField] private PhaseType phase;
        [SerializeField] private List<EnemyActionData> actions;

        public PhaseType Phase => phase;
        public List<EnemyActionData> Actions => actions;
    }
    
    [CreateAssetMenu(fileName = "New Enemy Phase Actions", menuName = "EnemyAI/Enemy Phase Actions")]
    public class EnemyPhaseActions : ScriptableObject
    {
        [SerializeField] private List<ActionsForPhase> actionsForPhases;

        private Dictionary<PhaseType, List<EnemyActionData>> phasesToActions;
        public IReadOnlyDictionary<PhaseType, List<EnemyActionData>> PhasesToActions => phasesToActions;

        private void OnEnable()
        {
            CheckValidity();

            phasesToActions = actionsForPhases.ToDictionary((obj) => obj.Phase, (obj) => obj.Actions);
        }

        private void CheckValidity()
        {
            foreach (PhaseType phase in Enum.GetValues(typeof(PhaseType)))
            {
                var phaseOccurrences = actionsForPhases.Count((obj) => obj.Phase == phase);
                switch (phaseOccurrences)
                {
                    case > 1:
                        Debug.LogError($"{name}: there is more than one data for {phase} phase");
                        break;
                    case < 1:
                        Debug.LogError($"{name}: there is no data for {phase} phase");
                        break;
                }
            }
        }
    }
}

