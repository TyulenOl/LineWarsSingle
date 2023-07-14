using System.Collections.Generic;
using System.IO;
using System.Linq;
using LineWars.Model;
using UnityEngine;


namespace LineWars.Controllers
{
    public class LevelsManager : MonoBehaviour
    {
        public static LevelsManager Instance { get; private set; }
        
        private MapData mapData;
        private GraphData graphData => mapData.GraphData;
        
        private void Awake()
        {
            Instance = this;
        }

        public static bool ExistenceMap(string mapName) => Instance._ExistenceMap(mapName);
        public static void LoadMap(string mapName) => Instance._LoadMap(mapName);
        private bool _ExistenceMap(string mapName)
        {
            var mapData = Resources.Load<MapData>($"{LevelsInfo.MAP_DIRECTORY_NAME}/{mapName}");
            return mapData != null && mapData.GraphData != null;
        }
        private void _LoadMap(string mapName)
        {
            mapData = Resources.Load<MapData>($"{LevelsInfo.MAP_DIRECTORY_NAME}/{mapName}");
            if (mapData == null || mapData.GraphData == null)
            {
                Debug.LogError($"Карты {mapName} не существует");
                return;
            }

            var drawer = new GraphDataDrawer();
            drawer.DrawGraph(null);
        }

    }
}