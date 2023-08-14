using System;
using UnityEditor;
using UnityEngine;

namespace LineWars.Controllers
{
    [CustomEditor(typeof(UnitBackgroundDrawer))]
    public class UnitBackgroundDrawerEditor : Editor
    {
        private UnitBackgroundDrawer drawer => (UnitBackgroundDrawer) target;
        private bool needInitialize = true;
        

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("DrawLeft"))
            {
                Initialize();
                drawer.DrawLeft();
            }

            if (GUILayout.Button("DrawCenter"))
            {
                Initialize();
                drawer.DrawCenter();
            }

            if (GUILayout.Button("DrawRight"))
            {
                Initialize();
                drawer.DrawRight();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void Initialize()
        {
            if (needInitialize)
            {
                needInitialize = false;
                drawer.Initialize();
            }
        }
    }
}