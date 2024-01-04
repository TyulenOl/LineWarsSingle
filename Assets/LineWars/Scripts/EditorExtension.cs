#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public static class EditorExtensions
{
    public static bool CanRedraw(Object obj)
    {
        return !PrefabUtility.IsPartOfPrefabAsset(obj)
               && !PrefabUtility.IsPartOfImmutablePrefab(obj)
               && PrefabUtility.IsPartOfPrefabInstance(obj)
               && !EditorApplication.isCompiling
               && !EditorApplication.isPlayingOrWillChangePlaymode
               && !EditorApplication.isUpdating
               && !EditorApplication.isTemporaryProject
               && Application.isEditor;
    }
}
#endif
