﻿using DataStructures;
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
        [field:SerializeField] public WinOrLoseAction WinOrLoseAction { get; set; }
        [field:SerializeField] public PlayerInitializer PlayerInitializer { get;  set; }
        
        [field: Header("Getters")]
        [field: SerializeField] public DeckGetter DeckGetter { get; set; }
        [field: SerializeField] public BlessingStorageGetter BlessingStorageGetter { get; set; }
        [field: SerializeField] public BlessingPullGetter LocalBlessingPullGetter { get; set; }
        [field: SerializeField] public BlessingPullGetter GlobalBlessingPullGetter { get; set; }
        
        [field: Header("Controllers")]
        [field: SerializeField] public CommandsManager CommandsManager { get; set; }
        
        public readonly IndexList<BasePlayer> AllPlayers = new();
        public readonly IndexList<Unit> AllUnits = new();
        public Deck CurrentDeck { get; private set; }
        public IBlessingsPull LocalBlessingPull { get; private set; }
        public IBlessingsPull GlobalBlessingPull { get; private set; }
        
        private void Start()
        {
            if (autoInitialize)
                StartGame();
        }

        public void StartGame()
        {
            LocalBlessingPull = LocalBlessingPullGetter.Get();
            GlobalBlessingPull = GlobalBlessingPullGetter.Get();
            
            InitializeDeck();

            InitializeAndRegisterAllPlayers();
            InitializeGameReferee();
            CommandsManager.Initialize(LocalBlessingPull, BlessingStorageGetter.Get());
            
            StartCoroutine(StartGameCoroutine());

            IEnumerator StartGameCoroutine()
            {
                yield return null;
                PhaseManager.Instance.StartGame();
            }
        }

        private void InitializeDeck()
        {
            if (DeckGetter == null)
                Debug.LogError("DeckGetter is null");
            if (DeckGetter.CanGet())
                CurrentDeck = DeckGetter.Get();
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
            {
                Debug.LogError($"Нет {nameof(GameReferee)}");
                return;
            }
            if (WinOrLoseAction == null)
                Debug.LogError($"Нет {nameof(Controllers.WinOrLoseAction)}");
            
            GameReferee.Initialize(Player.LocalPlayer, AllPlayers
                .Select(x => x.Value)
                .Where(x => x != Player.LocalPlayer));
            
            GameReferee.Wined += WinOrLoseAction.OnWin;
            GameReferee.Losed += WinOrLoseAction.OnLose;
        }

        [EditorButton]
        public void WinGame()
        {
            WinOrLoseAction.OnWin();
        }
        
        [EditorButton]
        public void LoseGame()
        {
            WinOrLoseAction.OnLose();
        }

        public void PauseGame()
        {
            
        }

        public void ResumeGame()
        {
            
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (SfxManager.Instance != null) SfxManager.Instance.StopAllSounds();
            if (SpeechManager.Instance != null) SpeechManager.Instance.StopAllSounds();
        }
    }
}