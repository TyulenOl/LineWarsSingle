// using LineWars.Model;
// using UnityEditor;
// using UnityEngine;
//
// [CustomEditor(typeof(GraphData))]
// public class GraphDataEditor : Editor
// {
//     private GraphData GraphData => (GraphData) target;
//
//     public override void OnInspectorGUI()
//     {
//         GUILayout.Label("Общая информация");
//         GUILayout.Space(10);
//         
//         EditorGUI.indentLevel++;
//         GUILayout.Label($"Колличество нод: {GraphData.NodesCount}");
//         GUILayout.Label($"Колличество ребер: {GraphData.EdgesCount}");
//         GUILayout.Label($"Колличество спавнов: {GraphData.SpawnCount}");
//
//         EditorGUI.indentLevel--;
//     }
// }