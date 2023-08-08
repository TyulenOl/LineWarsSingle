// using System.IO;
// using System.Linq;
// using LineWars;
// using LineWars.Controllers;
// using LineWars.Model;
// using UnityEditor;
// using UnityEngine;
//
//
// [CustomEditor(typeof(Graph))]
// public class GraphEditor : Editor
// {
//     private static string GraphDirectoryName => LevelsInfo.GRAPH_DIRECTORY_NAME;
//     private static string GraphFileName => LevelsInfo.GRAPH_FILE_NAME;
//
//     private DirectoryInfo assetsDirectory => LevelsInfo.AssetsDirectory;
//     private DirectoryInfo graphDirectory => LevelsInfo.GraphDirectory;
//     private Graph Graph => (Graph) target;
//     
//     private bool isLoaded;
//
//     private GraphData graphData;
//
//     private void OnEnable()
//     {
//         isLoaded = FindObjectsOfType<Node>().Length != 0 || FindObjectsOfType<Edge>().Length != 0;
//     }
//
//     public override void OnInspectorGUI()
//     {
//         base.OnInspectorGUI();
//
//         graphData = (GraphData) EditorGUILayout.ObjectField(
//             "Add GraphData",
//             graphData,
//             typeof(GraphData),
//             false
//         );
//         if (graphDirectory != null)
//         {
//             GUILayout.BeginHorizontal();
//             
//             if (GUILayout.Button("SaveNew"))
//                 SaveGraph();
//
//             EditorGUI.BeginDisabledGroup(graphData == null);
//             if (GUILayout.Button("Replace"))
//                 ReplaceGraph();
//             EditorGUI.EndDisabledGroup();
//
//             EditorGUI.BeginDisabledGroup(isLoaded || graphData == null);
//             if (GUILayout.Button("Load"))
//                 LoadGraph();
//             EditorGUI.EndDisabledGroup();
//
//             EditorGUI.BeginDisabledGroup(!isLoaded);
//             if (GUILayout.Button("Delete"))
//                 UnLoadGraph();
//             
//             EditorGUI.EndDisabledGroup();
//
//             GUILayout.EndHorizontal();
//         }
//         else
//         {
//             EditorGUILayout.LabelField("No Graph Directory");
//         }
//     }
//
//     private void LoadGraph()
//     {
//         if (isLoaded) return;
//
//         if (graphData != null)
//         {
//             var drawer = new GraphBuilder();
//             drawer.BuildGraph(graphData, true);
//             isLoaded = true;
//         }
//         else
//         {
//             EditorUtility.DisplayDialog("GraphData is empty!", "", "Ok");
//         }
//     }
//
//     private void UnLoadGraph()
//     {
//         foreach (var node in FindObjectsOfType<Node>())
//             DestroyImmediate(node.gameObject);
//         foreach (var edge in FindObjectsOfType<Edge>())
//             DestroyImmediate(edge.gameObject);
//         isLoaded = false;
//     }
//
//     private void SaveGraph()
//     {
//         var newGraphData = GetNewGraphData();
//         if (newGraphData == null)
//             return;
//         
//         var uniqueAssetFileName = GetUniqueRelativePath();
//         AssetDatabase.CreateAsset(newGraphData, uniqueAssetFileName);
//     }
//
//     private void ReplaceGraph()
//     {
//         var newGraphData = GetNewGraphData();
//         if (newGraphData == null)
//             return;
//         
//         graphData.RefreshFrom(newGraphData);
//         EditorUtility.SetDirty(graphData);
//     }
//
//     private string GetUniqueRelativePath()
//     {
//         var relativePath = Path.GetRelativePath(assetsDirectory.Parent.FullName, graphDirectory.FullName);
//         var assetFileName = Path.Join(relativePath, GraphFileName);
//
//         var index = 1;
//         var uniqueAssetFileName = $"{assetFileName}{index}.asset";
//
//         while (File.Exists(uniqueAssetFileName))
//         {
//             index++;
//             uniqueAssetFileName = $"{assetFileName}{index}.asset";
//         }
//
//         return uniqueAssetFileName;
//     }
//
//     private GraphData GetNewGraphData()
//     {
//         var allNodes = FindObjectsOfType<Node>().ToList();
//         allNodes.Reverse();
//         if (allNodes.Count == 0) return null;
//
//         var newGraphData = CreateInstance<GraphData>();
//         newGraphData.Initialize(allNodes);
//
//         return newGraphData;
//     }
// }