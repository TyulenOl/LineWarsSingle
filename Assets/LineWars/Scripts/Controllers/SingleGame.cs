using System;
using DataStructures;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LineWars.Interface;
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
        
        [Header("References")]
        [SerializeField] private PlayerInitializer playerInitializer;
        
        [Header("Debug")] 
        [SerializeField] private bool isAI;

        public readonly IndexList<BasePlayer> AllPlayers = new();
        public readonly IndexList<Unit> AllUnits = new();
        
        private Player player;
        

        private Stack<SpawnInfo> spawnInfosStack;
        private SpawnInfo playerSpawnInfo;
        
        public SceneName MyScene => (SceneName) SceneManager.GetActiveScene().buildIndex;
        private bool HasSpawnPoint() => spawnInfosStack.Count > 0;
        private SpawnInfo GetSpawnPoint() => spawnInfosStack.Pop();

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (playerInitializer == null)
                throw new ConstraintException($"{nameof(playerInitializer)} is null on {name}");
            StartGame();
        }

        private void StartGame()
        {
            InitializeSpawns();
            InitializePlayer();
            InitializeAIs();
            
            InitializeGameReferee();
            
            
            StartCoroutine(StartGameCoroutine());
            IEnumerator StartGameCoroutine()
            {
                yield return null;
                PhaseManager.Instance.StartGame();
            }
        }

        private void InitializeGameReferee()
        {
            if (GameReferee.Instance == null)
                Debug.LogError($"Нет {nameof(GameReferee)} на данной сцене");
            GameReferee.Instance.Initialize(Player.LocalPlayer, AllPlayers
                .Select(x => x.Value)
                .Where(x => x != Player.LocalPlayer));
            GameReferee.Instance.Wined += WinGame;
            GameReferee.Instance.Losed += LoseGame;
        }
        

        private void InitializeSpawns()
        {
            if (MonoGraph.Instance.Spawns.Count == 0)
            {
                Debug.LogError("Игрок не создался, потому что нет точек для его спавна");
                return;
            }

            playerSpawnInfo = playerSpawn
                ? MonoGraph.Instance.Spawns.First(info => info.SpawnNode == playerSpawn)
                : MonoGraph.Instance.Spawns.First();
            
            spawnInfosStack = MonoGraph.Instance.Spawns
                .Where(x => x != playerSpawnInfo)
                .ToStack(true);
        }

        private void InitializePlayer()
        { 
            player = playerInitializer.Initialize<Player>(playerSpawnInfo);
            player.RecalculateVisibility(false);
        }
        

        private void InitializeAIs()
        {
            while (HasSpawnPoint())
            {
                var spawnPoint = GetSpawnPoint();
                BasePlayer enemy = isAI
                    ? playerInitializer.Initialize<EnemyAI>(spawnPoint)
                    : playerInitializer.Initialize<TestActor>(spawnPoint); 

                //AllPlayers.Add(enemy);
            }
        }
        

        private void WinGame()
        {
            Debug.Log("<color=yellow>Вы Победили</color>");
            if (!GameVariables.IsNormalStart) return;
            WinLoseUI.isWin = true;
            SceneTransition.LoadScene(SceneName.WinOrLoseScene);
            CompaniesDataBase.ChooseMission.isCompleted = true;
            CompaniesDataBase.SaveChooseMission();
        }

        private void LoseGame()
        {
            Debug.Log("<color=red>Потрачено</color>");
            if (!GameVariables.IsNormalStart) return;
            WinLoseUI.isWin = false;
            SceneTransition.LoadScene(SceneName.WinOrLoseScene);
        }
    }
}