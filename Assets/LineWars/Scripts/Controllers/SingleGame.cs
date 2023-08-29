using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LineWars.Controllers;
using LineWars.Extensions;
using LineWars.Extensions.Attributes;
using LineWars.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LineWars
{
    public class SingleGame: MonoBehaviour
    {
        public static SingleGame Instance { get; private set; }
        [Header("Logic")]
        [SerializeField] private Spawn playerSpawn;
        [SerializeField] private int scoreForWin;
        
        [Header("References")]
        [SerializeField] private PlayerInitializer playerInitializer;
        [Header("Debug")] 
        [SerializeField] private bool isAI;
        [SerializeField, ReadOnlyInspector] private List<BasePlayer> players;

        private Stack<SpawnInfo> spawnInfosStack;
        private SpawnInfo playerSpawnInfo;

        public SceneName MyScene => (SceneName) SceneManager.GetActiveScene().buildIndex;
        public int ScoreForWin => scoreForWin;
        private bool HasSpawnPoint() => spawnInfosStack.Count > 0;
        private SpawnInfo GetSpawnPoint() => spawnInfosStack.Pop();
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            InitializeSpawns();
            InitializePlayer();
            InitializeAIs();
            StartCoroutine(StartGameCoroutine());
            IEnumerator StartGameCoroutine()
            {
                yield return null;
                PhaseManager.Instance.StartGame();
            }
        }

        private void InitializeSpawns()
        {
            if (Graph.Spawns.Count == 0)
            {
                Debug.LogError("Игрок не создался, потому что нет точек для его спавна");
                return;
            }

            playerSpawnInfo = playerSpawn
                ? Graph.Spawns.First(info => info.SpawnNode == playerSpawn)
                : Graph.Spawns.First();
            
            spawnInfosStack = Graph.Spawns
                .Where(x => x != playerSpawnInfo)
                .ToStack(true);
        }

        private void InitializePlayer()
        {
            var player = playerInitializer.Initialize<Player>(playerSpawnInfo);
            players.Add(player);
        }

        private void InitializeAIs()
        {
            while (HasSpawnPoint())
            {
                var spawnPoint = GetSpawnPoint();
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

        public void WinGame()
        {
            Debug.Log("Юхууу, вы выйграли!");
        }

        public void LoseGame()
        {
            Debug.Log("Потрачено");
        }

        public void ToMainMenu()
        {
            SceneTransition.LoadScene(SceneName.MainMenu);
        }
    }
}