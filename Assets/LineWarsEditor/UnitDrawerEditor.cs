using System;
using LineWars.Interface;
using LineWars.Model;
using UnityEditor;
using UnityEngine;

namespace LineWarsEditor
{
    [CustomEditor(typeof(UnitDrawer))]
    public class UnitDrawerEditor: Editor
    {
        private UnitDrawer unitDrawer;
        private Unit unit;

        private void OnEnable()
        {
            unitDrawer = (UnitDrawer) target;
            unit = unitDrawer.GetComponent<Unit>();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Assign DrawInfo"))
            {
                if (unit == null || unitDrawer == null)
                    return;
                AssignSprite();
                AssignName();
            }
        }

        private void AssignSprite()
        {
            if (unit.Sprite == null) return;
            
            if (unit.Size == UnitSize.Little
                && unitDrawer.LeftDrawer != null
                && unitDrawer.LeftDrawer.targetSprite != null
                && unitDrawer.LeftDrawer.targetSprite.sprite != unit.Sprite)
            {
                unitDrawer.LeftDrawer.targetSprite.sprite = unit.Sprite;
                EditorUtility.SetDirty(unitDrawer.LeftDrawer.targetSprite);
            }

            if (unitDrawer.RightDrawer != null
                && unitDrawer.RightDrawer.targetSprite != null
                && unitDrawer.RightDrawer.targetSprite.sprite != unit.Sprite)
            {
                unitDrawer.RightDrawer.targetSprite.sprite = unit.Sprite;
                EditorUtility.SetDirty(unitDrawer.RightDrawer.targetSprite);
            }
        }
        
        private void AssignName()
        {
            if (unit.Size == UnitSize.Little
                && unitDrawer.LeftDrawer != null
                && unitDrawer.LeftDrawer.UnitName != null
                && unitDrawer.LeftDrawer.UnitName.text != unit.UnitName)
            {
                unitDrawer.LeftDrawer.UnitName.text = unit.UnitName;
                EditorUtility.SetDirty(unitDrawer.LeftDrawer.UnitName);
            }

            if (unitDrawer.RightDrawer != null
                && unitDrawer.RightDrawer.UnitName != null
                && unitDrawer.RightDrawer.UnitName.text != unit.UnitName)
            {
                unitDrawer.RightDrawer.UnitName.text = unit.UnitName;
                EditorUtility.SetDirty(unitDrawer.RightDrawer.UnitName);
            }
        }
    }
}