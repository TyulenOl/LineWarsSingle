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
        if (EditorSceneManager.IsPreviewSceneObject(Edge.gameObject) || Application.isPlaying)
        {
            return;
        }

        try
        {
            Edge.Redraw();
            EditorUtility.SetDirty(Edge);
        }
        catch (Exception)
        {
            Debug.LogWarning("Произошла какая-то ошибка");
        }
    }
}