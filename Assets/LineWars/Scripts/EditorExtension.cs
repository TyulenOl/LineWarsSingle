#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class EditorExtensions
{
    public static bool CanRedraw(GameObject obj)
    {
        return !PrefabUtility.IsPartOfPrefabAsset(obj)
               && !PrefabUtility.IsPartOfImmutablePrefab(obj)
               && PrefabUtility.IsPartOfPrefabInstance(obj)
               && Application.isEditor
               && !EditorApplication.isCompiling
               && !EditorApplication.isPlayingOrWillChangePlaymode
               && !EditorApplication.isUpdating
               && !EditorApplication.isTemporaryProject
               && !EditorSceneManager.IsPreviewSceneObject(obj)
               && StageUtility.GetCurrentStage() is not PrefabStage;
    }
}
#endif
