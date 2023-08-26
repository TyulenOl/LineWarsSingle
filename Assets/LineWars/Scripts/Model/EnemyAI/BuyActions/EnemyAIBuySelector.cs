﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    public partial class EnemyAI
    {
        public class EnemyAIBuySelector
        {
            public bool TryGetPreset(EnemyAI enemyAI, [NotNullWhen(true)] out UnitBuyPreset buyPreset)
            {
                buyPreset = null;
                if (!enemyAI.Base.Node.AllIsFree) return false;

                var allPresets = enemyAI.nation.NationEconomicLogic.UnitBuyPresets;
                var currentMoney = enemyAI.CurrentMoney;
                float income = enemyAI.Income;
                var personality = enemyAI.personality;

                var maxValue = float.MinValue;
                foreach (var preset in allPresets)
                {
                    var basePresetValue =
                        GetUnitsByPreset(preset, enemyAI)
                            .Sum(unit => GetValueForUnit(unit, personality));
                    
                    var numberOfRoundsBeforeBuy = Mathf.CeilToInt(Mathf.Max(0, preset.Cost - currentMoney) / income);
                    
                    float presetValue = personality.InvestmentCoefficient == 0 && numberOfRoundsBeforeBuy > 0
                        ? 0
                        : basePresetValue / Mathf.Pow(numberOfRoundsBeforeBuy + 1, 1f / personality.InvestmentCoefficient);

                    if (presetValue > maxValue)
                    {
                        maxValue = presetValue;
                        buyPreset = numberOfRoundsBeforeBuy == 0 ? preset : null;
                    }
                }

                return buyPreset != null;
            }

            private float GetValueForUnit(Unit unit, EnemyAIPersonality personality)
            {
                var coef = personality.StrategyCoefficient;
                return unit.GetMyValue(personality) * (coef ? coef.GetValue(unit.Type) : 1);
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