using System;
using System.Linq;
using System.Collections.Generic;
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
        private Dictionary<PhaseType, IReadOnlyCollection<UnitType>> phaseToUnits;
        [NamedArray("phase")][SerializeField] private List<ExecutorsForPhase> executorsForPhases;

        public IReadOnlyDictionary<PhaseType, IReadOnlyCollection<UnitType>> PhaseToUnits => phaseToUnits;

        private void OnEnable()
        {
            CheckValidity();
            phaseToUnits = new Dictionary<PhaseType, IReadOnlyCollection<UnitType>>();

            phaseToUnits = executorsForPhases.ToDictionary((obj) => obj.Phase, (obj) => obj.Executors);
        }

        private void CheckValidity()
        {
            foreach (PhaseType phase in Enum.GetValues(typeof(PhaseType)))
            {
                var phaseOccurrences = executorsForPhases.Count((obj) => obj.Phase == phase);
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

        public IReadOnlyCollection<UnitType> this[PhaseType type] => PhaseToUnits[type];
    }
}

