using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LineWars;
using LineWars.Controllers;
using LineWars.Model;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Graph))]
public class GraphEditor : Editor
{
    private static string GraphDirectoryName => LevelsInfo.GRAPH_DIRECTORY_NAME;
    private static string GraphFileName => LevelsInfo.GRAPH_FILE_NAME;

    private DirectoryInfo assetsDirectory => LevelsInfo.AssetsDirectory;
    private DirectoryInfo graphDirectory => LevelsInfo.GraphDirectory;
    private Graph Graph => (Graph) target;

    private bool canSave;
    private bool isDraw;

    private GraphData graphData;

    private void OnEnable()
    {
        canSave = true;
        isDraw = FindObjectsOfType<Node>() != null || FindObjectsOfType<Edge>() != null;
        if (graphDirectory == null)
        { canSave = false; }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        graphData = (GraphData)EditorGUILayout.ObjectField(
            "Add GraphData",
            graphData,
            typeof(GraphData),
            false
        );

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            if (!canSave) return;
            SaveGraph();
        }

        if (GUILayout.Button("Draw"))
        {
            DrawGraph();
        }

        if (GUILayout.Button("Delete"))
        {
            DeleteGraph();
        }
        GUILayout.EndHorizontal();
    }

    private void DrawGraph()
    {
        if (isDraw) return;
        
        if (graphData != null)
        {
            var drawer = new GraphBuilder();
            drawer.BuildGraph(graphData);
            isDraw = true;
        }
        else
        {
            EditorUtility.DisplayDialog("GraphData is empty!", "", "Ok");
        }
    }

    private void DeleteGraph()
    {
        foreach (var node in FindObjectsOfType<Node>())
            DestroyImmediate(node.gameObject);
        foreach (var edge in FindObjectsOfType<Edge>())
            DestroyImmediate(edge.gameObject);
        isDraw = false;
    }

    private void SaveGraph()
    {
        var allNodes = FindObjectsOfType<Node>().ToArray();
        if (allNodes.Length == 0) return;

        var graphData = CreateInstance<GraphData>();
        
        graphData.Initialize(allNodes, GetSpawnNodes(allNodes));

        var uniqueAssetFileName = GetUniqueRelativePath();

        AssetDatabase.CreateAsset(graphData, uniqueAssetFileName);
    }

    private IReadOnlyList<Node> GetSpawnNodes(IReadOnlyList<Node> nodes)
    {
        return nodes.Where(node => node.IsSpawn).ToList();
    }

    private string GetUniqueRelativePath()
    {
        var relativePath = Path.GetRelativePath(assetsDirectory.Parent.FullName, graphDirectory.FullName);
        var assetFileName = Path.Join(relativePath, GraphFileName);

        var index = 1;
        var uniqueAssetFileName = $"{assetFileName}{index}.asset";

        while (File.Exists(uniqueAssetFileName))
        {
            index++;
            uniqueAssetFileName = $"{assetFileName}{index}.asset";
        }
        
        return uniqueAssetFileName;
    }
}