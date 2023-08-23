using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Extensions.Attributes;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class SingleGameManager: MonoBehaviour
    {
        public SingleGameManager Instance { get; private set; }
        [SerializeField] private PlayerInitializer playerInitializer;
        [Header("Debug")] [SerializeField] private bool isAI;
        [SerializeField, ReadOnlyInspector] private List<BasePlayer> players;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            InitializePlayer();
            InitializeAIs();
            StartCoroutine(StartGameCoroutine());
            IEnumerator StartGameCoroutine()
            {
                yield return null;
                PhaseManager.Instance.StartGame();
            }
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
                if (isAI)
                {
                    var player = playerInitializer.Initialize<EnemyAI>(spawnPoint);
                    players.Add(player);
                }
                else
                {
                    var testActor = playerInitializer.Initialize<TestActor>(spawnPoint);
                    players.Add(testActor);
                }
            }
        }
        

        public void ToMainMenu()
        {
            SceneTransition.LoadScene(SceneName.MainMenu);
        }
    }
}