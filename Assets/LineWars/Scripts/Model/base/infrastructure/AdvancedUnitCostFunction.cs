using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using org.mariuszgromada.math.mxparser;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "UnitCostFunctions/Advanced")]
    public class AdvancedUnitCostFunction : UnitCostFunction
    {
        [SerializeField] private string baseCostParameterName = "x";
        [SerializeField] private string unitCountParameterName = "y";

        [SerializeField, SerializedDictionary("Тип юнита", "Функция стоимости")]
        private SerializedDictionary<UnitType, string> functions;

        private readonly Dictionary<UnitType, (int, int, PurchaseInfo)> cash = new();

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
                    var x = new Argument($"{baseCostParameterName} = {baseCost}");
                    var y = new Argument($"{unitCountParameterName} = {unitCount}");
                    var expression = new Expression(stringExpression, x, y);
                    var purchaseInfo = new PurchaseInfo((int) expression.calculate());
                    cash[unitType] = (baseCost, unitCount, purchaseInfo);
                    return purchaseInfo;
                }
 
            }

            return new PurchaseInfo(baseCost);
        }
    }
}