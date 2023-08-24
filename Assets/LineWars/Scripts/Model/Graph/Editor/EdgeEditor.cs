using System;
using LineWars.Model;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(Edge))]
public class EdgeEditor: Editor
{
    private Edge Edge => (Edge)target;

    private void OnSceneGUI()
    {
        if (EditorSceneManager.IsPreviewSceneObject(Edge.gameObject))
        {
            return;
        }
        Edge.Redraw();
        EditorUtility.SetDirty(Edge);
    }
}