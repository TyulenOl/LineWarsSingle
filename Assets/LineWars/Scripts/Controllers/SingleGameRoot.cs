using DataStructures;
using System.Collections;
using System.Linq;
using LineWars.Model;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars
{
    public sealed class SingleGameRoot : Singleton<SingleGameRoot>
    {
        [SerializeField] private bool autoInitialize = true;
        
        [field:SerializeField] public GameReferee GameReferee { get;  set; }
        [SerializeField] private WinOrLoseAction winOrLoseAction;
        [field:SerializeField] public PlayerInitializer PlayerInitializer { get;  set; }
        
        public readonly IndexList<BasePlayer> AllPlayers = new();
        public readonly IndexList<Unit> AllUnits = new();
        
        private void Start()
        {
            if (autoInitialize)
                StartGame();
        }

        public void StartGame()
        {
            InitializeAndRegisterAllPlayers();
            InitializeGameReferee();
            
            StartCoroutine(StartGameCoroutine());

            IEnumerator StartGameCoroutine()
            {
                yield return null;
                PhaseManager.Instance.StartGame();
            }
        }
        private void InitializeAndRegisterAllPlayers()
        {
            foreach (var player in PlayerInitializer.InitializeAllPlayers())
                AllPlayers.Add(player.Id, player);
            
            foreach (var (key, value) in AllPlayers.Reverse())
            {
                PhaseManager.Instance.RegisterActor(value);
                Debug.Log($"{value.name} registered");
            }
        }

        private void InitializeGameReferee()
        {
            if (GameReferee == null)
                Debug.LogError($"Нет {nameof(GameReferee)}");
            if (winOrLoseAction == null)
                Debug.LogError($"Нет {nameof(WinOrLoseAction)}");
            
            GameReferee.Initialize(Player.LocalPlayer, AllPlayers
                .Select(x => x.Value)
                .Where(x => x != Player.LocalPlayer));
            
            GameReferee.Wined += winOrLoseAction.OnWin;
            GameReferee.Losed += winOrLoseAction.OnLose;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (SfxManager.Instance != null) SfxManager.Instance.StopAllSounds();
            if (SpeechManager.Instance != null) SpeechManager.Instance.StopAllSounds();
        }
    }
}