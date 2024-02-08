using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Mathos.Parser;

namespace LineWars.Model
{
    [Obsolete]
    [CreateAssetMenu(menuName = "UnitCostFunctions/Advanced")]
    public class AdvancedUnitCostFunction : UnitCostFunction
    {
        private MathParser mathParser;
        
        [SerializeField] private string baseCostParameterName = "x";
        [SerializeField] private string unitCountParameterName = "y";

        [SerializeField, SerializedDictionary("Тип юнита", "Функция стоимости")]
        private SerializedDictionary<UnitType, string> functions;

        private readonly Dictionary<UnitType, (int, int, PurchaseInfo)> cash = new();
        private readonly Dictionary<(string, int, int), PurchaseInfo> stringCash = new();

        private void OnEnable()
        {
            mathParser = new MathParser();
        }

        public override PurchaseInfo Calculate(UnitType unitType, int baseCost, int unitCount)
        {
            if (functions.TryGetValue(unitType, out var stringExpression))
            {
                if (cash.TryGetValue(unitType, out var unitHash)
                    && unitHash.Item1 == baseCost && unitHash.Item2 == unitCount)
                {
                    return unitHash.Item3;
                }
                else
                {
                    
                    mathParser.LocalVariables[baseCostParameterName] = baseCost;
                    mathParser.LocalVariables[unitCountParameterName] = unitCount;
                    
                    var purchaseInfo = new PurchaseInfo((int) mathParser.Parse(stringExpression));
                    cash[unitType] = (baseCost, unitCount, purchaseInfo);
                    return purchaseInfo;
                }
 
            }

            return new PurchaseInfo(baseCost);
        }
    }
}