using DataStructures;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LineWars.Interface;
using LineWars.Model;
using LineWars.Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LineWars
{
    public class SingleGame : Singleton<SingleGame>
    {
        [SerializeField] private bool autoInitialize = true;
        
        [field: SerializeField] public Player Player { get; set; }
        [field: SerializeField] public List<BasePlayer> Enemies { get; set; }

        private int currentPlayerIndex;
        
        public readonly IndexList<BasePlayer> AllPlayers = new();
        public readonly IndexList<Unit> AllUnits = new();
        
        public SceneName MyScene => (SceneName) SceneManager.GetActiveScene().buildIndex;
        
        private void Start()
        {
            if (autoInitialize)
                StartGame();
        }

        public void StartGame()
        {
            var player = InitializeBasePlayer(Player);
            player.RecalculateVisibility(false);
            AllPlayers.Add(player.Id, player);

            foreach (var enemiesInfo in Enemies)
            {
                var enemy = InitializeBasePlayer(enemiesInfo);
                AllPlayers.Add(enemy.Id, enemy);
            }
            
            InitializeGameReferee();
            
            RegisterAllPlayers();

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
        
        public void WinGame()
        {
            Debug.Log("<color=yellow>Вы Победили</color>");
            if (!GameVariables.IsNormalStart)
                return;
            WinLoseUI.isWin = true;
            GameRoot.Instance.CompaniesController.WinChoseMission();
            SceneTransition.LoadScene(SceneName.WinOrLoseScene);
        }

        public void LoseGame()
        {
            Debug.Log("<color=red>Потрачено</color>");
            if (!GameVariables.IsNormalStart)
                return;
            WinLoseUI.isWin = false;
            GameRoot.Instance.CompaniesController.DefeatChoseMission();
            SceneTransition.LoadScene(SceneName.WinOrLoseScene);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (SfxManager.Instance != null) SfxManager.Instance.StopAllSounds();
            if (SpeechManager.Instance != null) SpeechManager.Instance.StopAllSounds();
        }

        private T InitializeBasePlayer<T>(T player)
            where T : BasePlayer
        {
            InitializeBasePlayer(currentPlayerIndex, player);
            currentPlayerIndex++;
            return player;
        }
        
        private static void InitializeBasePlayer(int id, BasePlayer player)
        {
            player.Initialize(id);

            foreach (var spawn in player.InitialSpawns)
            {
                spawn.IsBase = true;
            }
            
            foreach (var node in player.AllInitialNodes)
            {
                node.IsDirty = true;
                
                Owned.Connect(player, node);

                var leftUnitPrefab = player.GetUnitPrefab(node.LeftUnitType);
                if (BasePlayerUtility.CanSpawnUnit(node, leftUnitPrefab, UnitDirection.Left))
                {
                    BasePlayerUtility.CreateUnitForPlayer(player, node, leftUnitPrefab, UnitDirection.Left);
                }

                var rightUnitPrefab = player.GetUnitPrefab(node.RightUnitType);
                if (BasePlayerUtility.CanSpawnUnit(node, rightUnitPrefab, UnitDirection.Right))
                {
                    BasePlayerUtility.CreateUnitForPlayer(player, node, rightUnitPrefab, UnitDirection.Right);
                }
            }
        }
    }
}