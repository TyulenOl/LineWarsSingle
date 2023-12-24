using System;
using System.Linq;
using LineWars.Model;
using UnityEditor;
using UnityEngine;
using Random = System.Random;


[CustomEditor(typeof(AdvancedUnitCostFunction))]
public class AdvancedUnitCostFunctionEditor : Editor
{
    public static Random random = new();
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
        try
        {
            foreach (var value in Enum.GetValues(typeof(UnitType)).OfType<UnitType>())
            {
                function.Calculate(value, random.Next(0, 100), random.Next(0, 100));
            }
            Debug.Log("Check success!");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}