using LineWars.Model;
using UnityEditor;
using UnityEngine;

namespace LineWarsEditor
{
    [CustomEditor(typeof(MonoGraph))]
    public class MonoGraphEditor: Editor
    {
        private MonoGraph MonoGraph => (MonoGraph) target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Reindex Nodes and Edges"))
            {
                var id = 0;
                foreach (var node in FindObjectsOfType<Node>())
                {
                    node.transform.SetAsLastSibling();
                    node.Id = id;
                    id++;
                    EditorUtility.SetDirty(node);
                }
                
                id = 0;
                foreach (var edge in FindObjectsOfType<Edge>())
                {
                    edge.transform.SetAsLastSibling();
                    edge.Id = id;
                    id++;
                    edge.Redraw();
                    EditorUtility.SetDirty(edge);
                }
                
                BasePlayer.RedrawAllPlayers();
            }
        }
    }
}