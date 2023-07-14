using System.IO;
using System.Linq;
using UnityEngine;

namespace LineWars.Controllers
{
    public static class LevelsInfo
    {
        public const string MAP_DIRECTORY_NAME = "Maps";
        public const string GRAPH_DIRECTORY_NAME = "Graphs";
        public const string GRAPH_FILE_NAME = "Graph";

        public static readonly DirectoryInfo AssetsDirectory;
        public static readonly DirectoryInfo MapDirectory;
        public static readonly DirectoryInfo GraphDirectory;
        
        static LevelsInfo()
        {
            AssetsDirectory = new DirectoryInfo(Application.dataPath);
            
            GraphDirectory = AssetsDirectory
                .EnumerateDirectories($"{GRAPH_DIRECTORY_NAME}", SearchOption.AllDirectories)
                .FirstOrDefault();

            MapDirectory = AssetsDirectory
                .EnumerateDirectories($"{MAP_DIRECTORY_NAME}", SearchOption.AllDirectories)
                .FirstOrDefault();
            
            if (GraphDirectory == null)
                Debug.LogError($"Ошибка папка {GRAPH_DIRECTORY_NAME} не найдена");
            
            if (MapDirectory == null)
                Debug.LogError($"Ошибка папка {MAP_DIRECTORY_NAME} не найдена");
        }
    }
}