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
        [field: SerializeField] public InitialPlayerInfo PlayerInfo { get; set; }
        [field: SerializeField] public List<InitialPlayerInfo> EnemiesInfos { get; set; }

        private int currentPlayerIndex;
        
        public readonly IndexList<BasePlayer> AllPlayers = new();
        public readonly IndexList<Unit> AllUnits = new();

        
        public SceneName MyScene => (SceneName) SceneManager.GetActiveScene().buildIndex;
        
        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            var player = InitializeBasePlayer<Player>(PlayerInfo);
            player.RecalculateVisibility(false);
            AllPlayers.Add(player.Id, player);

            foreach (var enemiesInfo in EnemiesInfos)
            {
                var enemy = InitializeBasePlayer<BasePlayer>(enemiesInfo);
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

        private T InitializeBasePlayer<T>(InitialPlayerInfo initialPlayerInfo)
            where T : BasePlayer
        {
            var player = (T) initialPlayerInfo.Player;
            InitializeBasePlayer(player, GetSpawnInfo(currentPlayerIndex, initialPlayerInfo));
            currentPlayerIndex++;
            return player;
        }
        
        private static void InitializeBasePlayer(BasePlayer player, SpawnInfo spawnInfo)
        {
            player.Initialize(spawnInfo);

            foreach (var spawn in spawnInfo.InitialSpawns)
            {
                spawn.IsDirty = true;
                spawn.IsBase = true;
            }
            
            foreach (var node in spawnInfo.Nodes)
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

        private static SpawnInfo GetSpawnInfo(int id, InitialPlayerInfo playerInfo)
        {
            return new SpawnInfo(
                id,
                playerInfo.MainSpawn,
                playerInfo.InitialSpawns,
                playerInfo.InitialNodes
            );
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (EditorExtensions.CanRedraw(this))
            {
                UnityEditor.EditorApplication.delayCall += DrawSpawns;
            }
        }
#endif
        private void DrawSpawns()
        {
            var initialInfos = new[] {PlayerInfo}.Concat(EnemiesInfos).ToArray();
            foreach (var initialInfo in initialInfos)
                initialInfo.Validate();
            
            foreach (var node in FindObjectsOfType<Node>())
                node.DrawToDefault();

            foreach (var initialInfo in initialInfos)
            {
                if (initialInfo.Nation == null)
                    continue;
                foreach (var spawn in initialInfo.InitialSpawns.Where(x => x != null))
                    spawn.Redraw(true, initialInfo.Nation.Name, initialInfo.Nation.NodeSprite);
                foreach (var node in initialInfo.InitialNodes.Where(x => x != null))
                    node.Redraw(false, initialInfo.Nation.Name, initialInfo.Nation.NodeSprite);
            }
        }
    }
}