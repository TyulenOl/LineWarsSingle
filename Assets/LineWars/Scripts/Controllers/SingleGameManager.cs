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
        [SerializeField, ReadOnlyInspector] private List<BasePlayer> players;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            InitializePlayer();
            InitializeAIs();
        }
        private void InitializePlayer()
        {
            if (Graph.HasSpawnPoint())
            {
                var spawnPoint = Graph.GetSpawnPoint();
                var player = playerInitializer.Initialize<Player>(spawnPoint);
                players.Add(player);
            }
            else
            {
                Debug.LogError("Игрок не создался, потому что нет точек для его спавна");
            }
        }

        private void InitializeAIs()
        {
            while (Graph.HasSpawnPoint())
            {
                var spawnPoint = Graph.GetSpawnPoint();
                var player = playerInitializer.Initialize<AI>(spawnPoint);
                players.Add(player);
            }
        }
        

        public void ToMainMenu()
        {
            SceneTransition.LoadScene(SceneName.MainMenu);
        }
    }
}