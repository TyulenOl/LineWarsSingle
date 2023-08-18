using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;

namespace LineWars.Model
{
    [CustomEditor(typeof(Nation))]
    public class NationEditor : Editor
    {
        private Nation Nation => (Nation) target;


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var newUnits = new List<(UnitType, Unit)>();
            var needUpdate = false;

            foreach (var (unitType, unit) in Nation.UnitTypeUnitPairs)
            {
                var box = EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(unitType.ToString());
                var newUnit = (Unit) EditorGUILayout.ObjectField(unit, typeof(Unit), false);
                if (newUnit != unit)
                {
                    needUpdate = true;
                    newUnits.Add((unitType, newUnit));
                }
                EditorGUILayout.EndHorizontal();
            }

            foreach (var (unitType, newUnit) in newUnits)
            {
                Nation.UnitTypeUnitPairs[unitType] = newUnit;
            }
            
            if (needUpdate)
                EditorUtility.SetDirty(Nation);
        }
    }
}