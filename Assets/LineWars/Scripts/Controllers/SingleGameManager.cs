using System;
using LineWars.Extensions.Attributes;
using UnityEngine;

namespace LineWars
{
    public class SingleGameManager: MonoBehaviour
    {
        public SingleGameManager Instance { get; private set; }
        [SerializeField, ReadOnlyInspector] private string mapToLoad;
        [SerializeField] private Player playerPrefab;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            mapToLoad = "TestMap";
            InitializeMap();
            InitializePlayer();
        }

        private void InitializeMap()
        {
            LevelsManager.LoadMap(mapToLoad);
        }
        
        private void InitializePlayer()
        {
            // можно перенести в класс PlayerBuilder
            if (LevelsManager.HasSpawnPoint())
            {
                var spawnPoint = LevelsManager.GetSpawnPoint();
                var player = Instantiate(playerPrefab);
                Owned.Connect(player, spawnPoint);
            }
            else
            {
                Debug.LogError("Игрок не создался, потому что нет точек для его спавна");
            }
        }

        public void ToMainMenu()
        {
            SceneTransition.LoadScene(SceneName.MainMenu);
        }
    }
}