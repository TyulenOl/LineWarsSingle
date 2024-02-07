using UnityEngine;
using System.Collections.Generic;
using Mathos.Parser;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "Function Cost", menuName = "UnitCost/Function Cost")]
    public class FunctionUnitCost : UnitCost
    {
        private MathParser mathParser;
        private Dictionary<(int, int), PurchaseInfo> cash;

        [SerializeField] private string baseCostParameterName = "x";
        [SerializeField] private string unitCountParameterName = "y";
        [SerializeField] private string progressionFunction;

        private void OnEnable()
        {
            mathParser = new MathParser();
            cash = new();
        }

        public override PurchaseInfo Calculate(int baseCost, int unitCount)
        {
            if(cash.ContainsKey((baseCost, unitCount)))
            {
                return cash[(baseCost, unitCount)];
            }
            mathParser.LocalVariables[baseCostParameterName] = baseCost;
            mathParser.LocalVariables[unitCountParameterName] = unitCount;

            var purchaseInfo = new PurchaseInfo((int)mathParser.Parse(progressionFunction));
            cash[(baseCost, unitCount)] = purchaseInfo;
            return purchaseInfo;
        }
    }
}
