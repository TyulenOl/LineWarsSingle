﻿using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PrefabOnlyAttribute))]
public class PrefabOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return property.isInstantiatedPrefab ? 0 : EditorGUI.GetPropertyHeight(property, label);
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.isInstantiatedPrefab) return;
        EditorGUI.PropertyField(position, property, label);
    }
}