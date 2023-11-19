using System;
using AYellowpaper.SerializedCollections;
using org.mariuszgromada.math.mxparser;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(menuName = "UnitCostFunctions/Advanced", order = 80)]
    public class AdvancedUnitCostFunction : UnitCostFunction
    {
        [SerializeField] private string baseCostParameterName = "x";
        [SerializeField] private string unitCountParameterName = "y";

        [SerializeField, SerializedDictionary("Тип юнита", "Функция стоимости")]
        private SerializedDictionary<UnitType, string> functions;

        public override PurchaseInfo Calculate(UnitType unitType, int baseCost, int unitCount)
        {
            if (functions.TryGetValue(unitType, out var stringExpression))
            {
                var x = new Argument($"{baseCostParameterName} = {baseCost}");
                var y = new Argument($"{unitCountParameterName} = {unitCount}");
                var expression = new Expression(stringExpression, x, y);
                return new PurchaseInfo((int) expression.calculate());
            }

            return new PurchaseInfo(baseCost);
        }
    }
}