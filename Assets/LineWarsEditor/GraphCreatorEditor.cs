using LineWars.Model;
using UnityEditor;
using UnityEngine;

namespace GraphEditor
{
    [CustomEditor(typeof(GraphCreator))]
    public class GraphCreatorEditor : Editor
    {
        private GraphCreator GraphCreator => (GraphCreator) target;
        private bool iterateStarted;
        private bool hardIterateStarted;
        
        private void OnEnable()
        {
            EditorApplication.update += Update;
        }
        
        private void OnDisable()
        {
            EditorApplication.update -= Update;
        }
        
        private void Update()
        {
            if (iterateStarted)
            {
                GraphCreator.SimpleIterate();
                GraphCreator.RedrawAllEdges();
            }
            else if (hardIterateStarted)
            {
                GraphCreator.HardIterate();
                GraphCreator.RedrawAllEdges();
            }
        }
        
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        
            if (GUILayout.Button("Restart"))
            {
                foreach (var g in FindObjectsOfType<MonoGraph>())
                {
                    DestroyImmediate(g.gameObject);
                }
        
                GraphCreator.Restart();
                GraphCreator.RedrawAllEdges();
                GraphCreator.DrawBorder();
                iterateStarted = false;
                hardIterateStarted = false;
            }
        
            if (GUILayout.Button("Start Simple Iterate"))
            {
                iterateStarted = true;
                hardIterateStarted = false;
            }
        
            
            if (GUILayout.Button("Start Hard Iterate"))
            {
                hardIterateStarted = true;
                iterateStarted = false;
            }
        
            if (GUILayout.Button("Stop Iterate"))
            {
                iterateStarted = false;
                hardIterateStarted = false;
            }
            
            if (GUILayout.Button("Delete Intersects by Count"))
            {
                Undo.IncrementCurrentGroup();
                GraphCreator.DeleteIntersectingEdgesByIntersectionsCount();
            }
            
            if (GUILayout.Button("Delete Intersects by Length"))
            {
                Undo.IncrementCurrentGroup();
                GraphCreator.DeleteIntersectingEdgesByLength();
            }
        }
    }
}