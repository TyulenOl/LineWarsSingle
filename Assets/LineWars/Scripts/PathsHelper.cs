using System.IO;
using UnityEngine;

namespace LineWars
{
    public static class PathsHelper
    {
        public static readonly string RootPathForSaves = Path.Combine(Application.persistentDataPath, "Saves");
        
        public static string GenerateKeyForPlayerPrefs<T>(int id)
        {
            return $"{nameof(T)}_{id}";
        }
    }
}