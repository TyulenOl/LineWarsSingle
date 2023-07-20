using System;
using System.Collections.Generic;
using LineWars.Extensions.Attributes;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class SingleGameManager: MonoBehaviour
    {
        public SingleGameManager Instance { get; private set; }
        [SerializeField] private PlayerInitializer playerInitializer;
        [SerializeField, ReadOnlyInspector] private string mapToLoad;

        [SerializeField, ReadOnlyInspector] private List<Player> players;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            mapToLoad = "TestMap";
            InitializeMap();
            InitializePlayer();

            // var visibilityInfo = Graph.Instance.GetVisibilityInfo(Player.LocalPlayer);
            // for (int i = 0; i < Graph.Instance.AllNodes.Count; i++)
            // {
            //     var isVisible = visibilityInfo[i];
            //     var node = Graph.Instance.AllNodes[i];
            //     if (isVisible)
            //     {
            //         node.GetComponent<SpriteRenderer>().color = Color.yellow;
            //     }
            // }
        }

        private void InitializeMap()
        {
            LevelsManager.LoadMap(mapToLoad);
        }
        
        private void InitializePlayer()
        {
            if (LevelsManager.HasSpawnPoint())
            {
                var spawnPoint = LevelsManager.GetSpawnPoint();
                var player = playerInitializer.Initialize(spawnPoint);
                players.Add(player);
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