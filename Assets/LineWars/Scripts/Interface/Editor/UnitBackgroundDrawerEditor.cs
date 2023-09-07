using System;
using UnityEditor;
using UnityEngine;

namespace LineWars.Interface
{
    [CustomEditor(typeof(UnitDrawer))]
    public class UnitBackgroundDrawerEditor : Editor
    {
        private UnitDrawer drawer => (UnitDrawer) target;


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("DrawLeft"))
            {
                drawer.DrawLeft();
            }

            if (GUILayout.Button("DrawCenter"))
            {
                drawer.DrawCenter();
            }

            if (GUILayout.Button("DrawRight"))
            {
                drawer.DrawRight();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}