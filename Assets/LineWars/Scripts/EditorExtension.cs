#if UNITY_EDITOR
using System.Text.RegularExpressions;
using LineWars;
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


    public static string GetAssetResourcePath(Object asset)
    {
        if (asset == null)
            return "";
        
        var assetPath = AssetDatabase.GetAssetPath(asset);
        if (assetPath.Contains("Resources"))
        {
            var regex = new Regex("^Assets/.*Resources/(?<relative_path>.+?)$",
                RegexOptions.RightToLeft);
            var matches = regex.Matches(assetPath);

            if (matches.Count == 0)
            {
                return string.Empty;
            }

            return matches[^1].Groups["relative_path"].Value.Split(".")[0];
        }

        return "";
    }
}
#endif
