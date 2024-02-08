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
                && unitDrawer.LeftDrawer.UnitSprite != null
                && unitDrawer.LeftDrawer.UnitSprite.sprite != unit.Sprite)
            {
                unitDrawer.LeftDrawer.UnitSprite.sprite = unit.Sprite;
                EditorUtility.SetDirty(unitDrawer.LeftDrawer.UnitSprite);
            }

            if (unitDrawer.RightDrawer != null
                && unitDrawer.RightDrawer.UnitSprite != null
                && unitDrawer.RightDrawer.UnitSprite.sprite != unit.Sprite)
            {
                unitDrawer.RightDrawer.UnitSprite.sprite = unit.Sprite;
                EditorUtility.SetDirty(unitDrawer.RightDrawer.UnitSprite);
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