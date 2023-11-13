using System;
using LineWars.Model;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(Node))]
public class NodeEditor : Editor
{
    private Node Node => (Node) target;


    private void OnSceneGUI()
    {
        if (EditorSceneManager.IsPreviewSceneObject(Node.gameObject) || Application.isPlaying)
        {
            return;
        }
    
        try
        {
            Node.Redraw();
            foreach (var edge in Node.Edges)
            {
                if(edge == null)
                {
                    Debug.LogWarning("В ноде присутствует пустое ребро! Перед началом игры убедитесь, что вы его убрали!", Node);
                    continue;
                }
                edge.Redraw();
                EditorUtility.SetDirty(edge);
            }
    
            EditorUtility.SetDirty(Node);
        }
        catch (Exception)
        {
            Debug.LogWarning("Произошла какая-то ошибка");
        }
    }
}