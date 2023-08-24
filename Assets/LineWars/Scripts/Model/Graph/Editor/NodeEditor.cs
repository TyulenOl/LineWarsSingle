using LineWars.Model;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(Node))]
public class NodeEditor : Editor
{
    private Node Node => (Node) target;

    public void OnSceneGUI()
    {
        if (EditorSceneManager.IsPreviewSceneObject(Node.gameObject))
        {
            return;
        }
        
        Node.Redraw();
        foreach (var edge in Node.Edges)
        {
            edge.Redraw();
            EditorUtility.SetDirty(edge);
        }

        EditorUtility.SetDirty(Node);
    }
}