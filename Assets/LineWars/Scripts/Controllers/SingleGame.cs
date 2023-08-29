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
        
        private List<BasePlayer> allPlayers = new ();
        private Player player;
        private int enemyCount = 0;
        

        private Stack<SpawnInfo> spawnInfosStack;
        private SpawnInfo playerSpawnInfo;

        public SceneName MyScene => (SceneName) SceneManager.GetActiveScene().buildIndex;
        public int ScoreForWin => scoreForWin;
        private bool HasSpawnPoint() => spawnInfosStack.Count > 0;
        private SpawnInfo GetSpawnPoint() => spawnInfosStack.Pop();
        private bool IsWinScore(int after) => after >= scoreForWin;

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
            player = playerInitializer.Initialize<Player>(playerSpawnInfo);
            player.Defeaded += PlayerOnDefeaded;
            player.ScoreChanged += PlayerOnScoreChanged;
            allPlayers.Add(player);
        }

        private void PlayerOnScoreChanged(int before, int after)
        {
            if (IsWinScore(after))
                WinGame();
        }
        
        private void PlayerOnDefeaded()
        {
            LoseGame();
        }

        private void InitializeAIs()
        {
            while (HasSpawnPoint())
            {
                var spawnPoint = GetSpawnPoint();
                BasePlayer enemy = isAI
                    ? playerInitializer.Initialize<EnemyAI>(spawnPoint)
                    : playerInitializer.Initialize<TestActor>(spawnPoint); 

                allPlayers.Add(enemy);
                enemyCount++;
                enemy.Defeaded += EnemyOnDefeaded;
                enemy.ScoreChanged += EnemyOnScoreChanged;
            }
        }

        private void EnemyOnScoreChanged(int before, int after)
        {
            if (IsWinScore(after))
                LoseGame();
        }

        private void EnemyOnDefeaded()
        {
            enemyCount--;
            if (enemyCount == 0)
                WinGame();
        }

        private void WinGame()
        {
            Debug.Log("Юхууу, вы выйграли!");
            WinLoseUI.isWin = true; //Пока так
            SceneTransition.LoadScene(SceneName.WinOrLoseScene);
            CompaniesDataBase.ChooseMission.isCompleted = true;
            CompaniesDataBase.SaveChooseMission();
        }

        private void LoseGame()
        {
            Debug.Log("Потрачено");
            WinLoseUI.isWin = false;
            SceneTransition.LoadScene(SceneName.WinOrLoseScene);
        }

        public void ToMainMenu()
        {
            SceneTransition.LoadScene(SceneName.MainMenu);
        }
    }
}