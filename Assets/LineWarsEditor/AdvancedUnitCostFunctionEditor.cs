using System;
using System.Linq;
using LineWars.Model;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(AdvancedUnitCostFunction))]
public class AdvancedUnitCostFunctionEditor : Editor
{
    private AdvancedUnitCostFunction Function => (AdvancedUnitCostFunction) target;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Check Valid"))
        {
            ValidateFunction(Function);
        }
    }

    private void ValidateFunction(AdvancedUnitCostFunction function)
    {
        foreach (var value in Enum.GetValues(typeof(UnitType)).OfType<UnitType>())
        {
            try
            {
                var baseCost = 100;
                var unitsCount = 0;

                var info = function.Calculate(value, baseCost, unitsCount);
                Debug.Log($"unitType={value} baseCost={baseCost} unitCount={unitsCount} result={info.Cost}");

                unitsCount = 1;
                info = function.Calculate(value, baseCost, unitsCount);
                Debug.Log($"unitType={value} baseCost={baseCost} unitCount={unitsCount} result={info.Cost}");

                unitsCount = UnityEngine.Random.Range(0, 100);
                info = function.Calculate(value, baseCost, unitsCount);
                Debug.Log($"Random unitType={value} baseCost={baseCost} unitCount={unitsCount} result={info.Cost}");
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message} {value}");
            }
        }
    }
}