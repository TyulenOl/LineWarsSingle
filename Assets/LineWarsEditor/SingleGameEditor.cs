// using System;
// using System.Collections.Generic;
// using LineWars;
// using LineWars.Model;
// using NUnit.Framework;
// using UnityEditor;
//
// namespace LineWarsEditor
// {
//     [CustomEditor(typeof(SingleGame))]
//     public class SingleGameEditor: Editor
//     {
//         private SingleGame SingleGame => (SingleGame) target;
//         private SerializedProperty autoRedraw;
//
//         private List<Node> spawns;
//         private List<>
//         
//         private void OnEnable()
//         {
//             autoRedraw = serializedObject.FindProperty("autoRedraw");
//         }
//
//         public override void OnInspectorGUI()
//         {
//             base.OnInspectorGUI();
//             if (!autoRedraw.boolValue)
//                 return;
//             
//             
//         }
//     }
// }