using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    public partial class EnemyAI
    {
        public class EnemyAIBuySelector
        {
            public UnitBuyPreset GetPreset(EnemyAI enemyAI)
            {
                if (!enemyAI.Base.Node.AllIsFree) return null;

                var allPresets = enemyAI.nation.NationEconomicLogic.UnitBuyPresets;
                var currentMoney = enemyAI.CurrentMoney;
                var income = enemyAI.Income;
                var personality = enemyAI.personality;

                var maxValue = float.MinValue;
                UnitBuyPreset result = null;
                foreach (var preset in allPresets)
                {
                    var basePresetValue = GetUnitsByPreset(preset, enemyAI)
                        .Sum(unit => GetValueForUnit(unit, personality));
                    var numberOfRoundsBeforeBuy = Mathf.Max(0, preset.Cost - currentMoney) / income;
                    float presetValue = personality.InvestmentCoefficient == 0 && numberOfRoundsBeforeBuy > 0
                        ? 0
                        : basePresetValue / Mathf.Pow(numberOfRoundsBeforeBuy + 1,
                            1 / personality.InvestmentCoefficient);

                    if (presetValue > maxValue)
                    {
                        maxValue = presetValue;
                        result = preset;
                    }
                }

                return result;
            }

            private float GetValueForUnit(Unit unit, EnemyAIPersonality personality)
            {
                return Base() * personality.StrategyCoefficient.GetValue(unit.Type);

                float Base()
                {
                    return (unit.MaxHp + unit.InitialArmor) * personality.Defensiveness
                           + (unit.MeleeDamage) * personality.Aggressiveness;
                }
            }

            private IEnumerable<Unit> GetUnitsByPreset(UnitBuyPreset preset, EnemyAI enemyAI)
            {
                var leftUnit = enemyAI.GetUnitPrefab(preset.FirstUnitType);
                if (leftUnit != null)
                    yield return leftUnit;
                var rightUnit = enemyAI.GetUnitPrefab(preset.SecondUnitType);
                if (rightUnit != null)
                    yield return rightUnit;
            }
        }
    }
}