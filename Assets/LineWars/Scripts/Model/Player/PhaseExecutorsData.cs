using System;
using System.Linq;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    [System.Serializable]
    public class ExecutorsForPhase
    {
        [SerializeField] private PhaseType phase;
        [SerializeField] private List<UnitType> executors;

        public PhaseType Phase => phase;
        public IReadOnlyCollection<UnitType> Executors => executors;
    } 

    [CreateAssetMenu(fileName = "New Phase Executors", menuName = "Phase Executors Data")]
    public class PhaseExecutorsData : ScriptableObject
    {
        [SerializeField] private List<ExecutorsForPhase> executorsForPhases;

        public Dictionary<PhaseType, IReadOnlyCollection<UnitType>> PhaseToUnits {get; private set;}

        private void OnEnable() 
        {
            CheckValidity();
            PhaseToUnits = new Dictionary<PhaseType, IReadOnlyCollection<UnitType>>();
            
            PhaseToUnits = executorsForPhases.ToDictionary((obj) => obj.Phase, (obj) => obj.Executors);
        }

        private void CheckValidity()
        {
            foreach(PhaseType phase in Enum.GetValues(typeof(PhaseType)))
            {
                var phaseOccurrences = executorsForPhases.Count((obj) => obj.Phase == phase);
                switch(phaseOccurrences)
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

