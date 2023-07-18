using System.Collections.Generic;
using System.IO;
using System.Linq;
using LineWars.Extensions;
using LineWars.Model;
using UnityEngine;


namespace LineWars
{
    public class LevelsManager : MonoBehaviour
    {
        public static LevelsManager Instance { get; private set; }
        
        private MapData mapData;
        private GraphData graphData => mapData.GraphData;
        private Stack<Node> spawnPointStack;

        private void Awake()
        {
            Instance = this;
        }

        public static bool ExistenceMap(string mapName) => Instance._ExistenceMap(mapName);
        public static void LoadMap(string mapName) => Instance._LoadMap(mapName);
        public static bool HasSpawnPoint() => Instance._HasSpawnPoint();
        public static Node GetSpawnPoint() => Instance._GetSpawnPoint();

        
        private Node _GetSpawnPoint() => spawnPointStack.Pop();
        private bool _HasSpawnPoint() => spawnPointStack.Count != 0;
        private bool _ExistenceMap(string mapName)
        {
            var mapData = Resources.Load<MapData>($"{LevelsInfo.MAP_DIRECTORY_NAME}/{mapName}");
            return mapData != null && mapData.GraphData != null;
        }
        private void _LoadMap(string mapName)
        {
            mapData = Resources.Load<MapData>($"{LevelsInfo.MAP_DIRECTORY_NAME}/{mapName}");
            if (mapData == null )
            {
                Debug.LogError($"Карты {mapName} не существует");
                return;
            }

            if (mapData.GraphData == null)
            {
                Debug.LogError($"У карты {mapName} нет графа");
                return;
            }

            var drawer = new GraphBuilder();
            var graph = drawer.BuildGraph(graphData);
            // spawnPointStack = graph.SpawnNodes
            //     .ToStack();
        }
    }
}