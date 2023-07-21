using LineWars.Model;
using UnityEditor;
using UnityEngine;

namespace LineWars
{
    [CustomEditor(typeof(NodeInitialInfo))]
    public class NodeInitialInfoEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            // serializedObject.Update();
            //
            // var isSpawnProperty = serializedObject.FindProperty("isSpawn");
            // EditorGUILayout.PropertyField(isSpawnProperty);
            // var isSpawn = isSpawnProperty.boolValue;
            // if (!isSpawn)
            // {
            //     var referenceToSpawnProperty = serializedObject.FindProperty("referenceToSpawn");
            //     EditorGUILayout.PropertyField(referenceToSpawnProperty);
            // }
            //
            // var leftUnitPrefabProperty = serializedObject.FindProperty("leftUnitPrefab");
            // var rightUnitPrefabProperty = serializedObject.FindProperty("rightUnitPrefab");
            //
            // EditorGUILayout.Space(5);
            // EditorGUILayout.LabelField("Левый юнит будет создаваться в приоритете!");
            // EditorGUILayout.PropertyField(leftUnitPrefabProperty);
            // EditorGUILayout.PropertyField(rightUnitPrefabProperty);
            //
            // serializedObject.ApplyModifiedProperties();
        }
    }
}