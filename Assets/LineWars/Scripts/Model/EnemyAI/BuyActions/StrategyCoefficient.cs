using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using JetBrains.Annotations;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New StrategyCoefficient", menuName = "EnemyAI/DynamicCoefficient/Create EnemyPersonality")]
    public class StrategyCoefficient : ScriptableObject
    {
        [SerializeField, SerializedDictionary("Phase", "StrategyInfo")]
        private SerializedDictionary<StrategyPhase, UnitTypeCoefficientDictionary> strategyInfosMap;

        [SerializeField, SerializedDictionary("Phase", "Start")]
        private SerializedDictionary<StrategyPhase, Coefficient> startPhaseValuesMap;

        private void OnValidate()
        {
            AssignStartPhaseValues();
            AssignStrategyInfoValues();
            
            void AssignStrategyInfoValues()
            {
                foreach (var phase in Enum.GetValues(typeof(StrategyPhase))
                             .OfType<StrategyPhase>())
                {
                    strategyInfosMap.TryAdd(phase, new UnitTypeCoefficientDictionary());
                }
            }

            void AssignStartPhaseValues()
            {
                foreach (var phase in Enum.GetValues(typeof(StrategyPhase))
                             .OfType<StrategyPhase>())
                    startPhaseValuesMap.TryAdd(phase, new Coefficient());

                startPhaseValuesMap[StrategyPhase.Start] = 0;
                foreach (var phase in Enum.GetValues(typeof(StrategyPhase))
                             .OfType<StrategyPhase>().Skip(1))
                    startPhaseValuesMap[phase] = Math.Max(startPhaseValuesMap[phase - 1], startPhaseValuesMap[phase]);
            }
        }

        public float GetValue(UnitType unitType)
        {
            var currentPhase = GetCurrentPhase(new GameStateHelper());
            if (strategyInfosMap.TryGetValue(currentPhase, out var unitTypeCoefficientDictionary)
                && unitTypeCoefficientDictionary.pairs.TryGetValue(unitType, out var value))
            {
                return value;
            }
            else
            {
                Debug.LogWarning($"Коэфициент стратегии {name} не содержит значения на {currentPhase}->{unitType}");
                return 0;
            }
        }

        protected virtual StrategyPhase GetCurrentPhase([NotNull] GameStateHelper stateHelper)
        {
            if (stateHelper == null) throw new ArgumentNullException(nameof(stateHelper));
            float percentOfMapCapture = stateHelper.GetGlobalPercentOfMapCapture();
            var result = StrategyPhase.Start;
            foreach (var (phase, start)in startPhaseValuesMap)
            {
                if (start <= percentOfMapCapture)
                    result = phase;
                else
                    break;
            }

            return result;
        }
    }

    [Serializable]
    public class UnitTypeCoefficientDictionary
    {
        [SerializedDictionary("UnitType", "Coefficient")]
        public SerializedDictionary<UnitType, Coefficient> pairs;

        public UnitTypeCoefficientDictionary()
        {
            pairs = new SerializedDictionary<UnitType, Coefficient>();
            foreach (var strategyPhase in Enum.GetValues(typeof(UnitType))
                         .OfType<UnitType>()
                         .Where(x => x!= UnitType.None))
                pairs.Add(strategyPhase, 0);
        }
    }

    [Serializable]
    public class Coefficient
    {
        [Range(0, 1)] public float coefficient;
        
        public static implicit operator Coefficient(float value)
        {
            return new Coefficient() {coefficient = value};
        }

        public static implicit operator float(Coefficient value)
        {
            return value.coefficient;
        }
    }
    public enum StrategyPhase
    {
        Start,
        Middle,
        End
    }
}