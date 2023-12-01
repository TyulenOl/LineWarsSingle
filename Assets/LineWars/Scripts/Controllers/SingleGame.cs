using System;
using DataStructures;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LineWars.Interface;
using LineWars.Model;
using LineWars.Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LineWars
{
    public class SingleGame : MonoBehaviour
    {
        public static SingleGame Instance { get; private set; }

        [Header("Logic")] [SerializeField] private PlayerBuilder playerSpawn;

        [Header("Players")] [SerializeField] private Player playerPrefab;
        [SerializeField] private List<BasePlayer> enemiesPrefabs;
        [SerializeField] private AIType aiType;

        [Header("AI Options")] [SerializeField]
        private GameEvaluator gameEvaluator;

        [SerializeField] private AIBuyLogicData aiBuyLogicData;
        [SerializeField, Min(1)] private int aiDepth;

        public readonly IndexList<BasePlayer> AllPlayers = new();
        public readonly IndexList<Unit> AllUnits = new();

        private PlayerInitializer playerInitializer = new();
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
            
            RegisterAllPlayers();
            //RegisterActors();

            StartCoroutine(StartGameCoroutine());

            IEnumerator StartGameCoroutine()
            {
                yield return null;
                PhaseManager.Instance.StartGame();
            }
        }

        private void RegisterAllPlayers()
        {
            foreach (var (key, value) in AllPlayers.Reverse())
            {
                PhaseManager.Instance.RegisterActor(value);
                Debug.Log($"{value.name} registered");
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

            if (playerSpawn != null)
            {
                playerSpawnInfo = MonoGraph.Instance.Spawns
                    .FirstOrDefault(info => info.SpawnNode == playerSpawn);
            }
            else
            {
                Debug.LogWarning("Ой ей вы не настроили спавн игрока");
                playerSpawnInfo = MonoGraph.Instance.Spawns.FirstOrDefault();
            }

            if (playerSpawnInfo == null)
            {
                throw new InvalidOperationException("Игра не может быть продолжена, так нет свавна для игрока");
            }

            spawnInfosStack = MonoGraph.Instance.Spawns
                .Where(x => x != playerSpawnInfo)
                .ToStack(true);
        }

        private void InitializePlayer()
        {
            player = playerInitializer.Initialize(playerPrefab, playerSpawnInfo);
            player.RecalculateVisibility(false);
            AllPlayers.Add(player.Id, player);
        }


        private void InitializeAIs()
        {
            while (HasSpawnPoint())
            {
                var spawnPoint = GetSpawnPoint();
                var enemy = playerInitializer.Initialize(
                    enemiesPrefabs.First(x => x.GetType() == aiType.ToType()),
                    spawnPoint);

                if (enemy is EnemyAI enemyAI)
                    InitializeAISettings(enemyAI);

                AllPlayers.Add(enemy.Id, enemy);
            }
        }

        private void InitializeAISettings(EnemyAI enemyAI)
        {
            if (gameEvaluator != null)
                enemyAI.SetNewGameEvaluator(gameEvaluator);
            if (aiBuyLogicData != null)
                enemyAI.SetNewBuyLogic(aiBuyLogicData);
            if (aiDepth > 0)
                enemyAI.Depth = aiDepth;
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

    public enum AIType
    {
        EnemyAI,
        TestActor,
        ProgrammedAI
    }

    public static class AITypeExtensions
    {
        private static readonly Dictionary<AIType, Type> aiTypeToTypeMap
            = new()
            {
                {AIType.EnemyAI, typeof(EnemyAI)},
                {AIType.TestActor, typeof(TestActor)},
                //{AIType.ProgrammedAI, typeof(ProgrammedAI)}
            };

        public static Type ToType(this AIType aiType)
        {
            if (aiTypeToTypeMap.TryGetValue(aiType, out var type))
                return type;
            throw new IndexOutOfRangeException();
        }
    }
}