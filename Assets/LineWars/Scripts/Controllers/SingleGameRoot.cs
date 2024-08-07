﻿using System;
using DataStructures;
using System.Collections;
using System.Linq;
using LineWars.Model;
using LineWars.Controllers;
using LineWars.Infrastructure;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LineWars
{
    public sealed class SingleGameRoot : Singleton<SingleGameRoot>
    {
        [SerializeField] private bool autoInitialize = true;
        
        [field:SerializeField] public GameReferee GameReferee { get;  set; }
        [field:SerializeField] public WinOrLoseAction WinAction { get; set; }
        [field:SerializeField] public WinOrLoseAction LoseAction { get; set; }
        [field:SerializeField] public PlayerInitializer PlayerInitializer { get;  set; }
        
        [field: Header("Getters")]
        [field: SerializeField] public DeckGetter DeckGetter { get; set; }
        [field: SerializeField] public BlessingStorageGetter BlessingStorageGetter { get; set; }
        [field: SerializeField] public BlessingPullGetter LocalBlessingPullGetter { get; set; }
        [field: SerializeField] public BlessingPullGetter GlobalBlessingPullGetter { get; set; }
        
        [field: Header("Controllers")]
        [field: SerializeField] public CommandsManager CommandsManager { get; set; }

        [Header("Debug")]
        [SerializeField] private int currentRound;
        
        public int CurrentRound
        {
            get => currentRound;
            private set
            {
                currentRound = value;
                RoundChanged?.Invoke(value);
            }
        }

        public readonly IndexList<BasePlayer> AllPlayers = new();
        public readonly IndexList<Unit> AllUnits = new();
     
        public Deck CurrentDeck { get; private set; }
        public IBlessingsPull LocalBlessingPull { get; private set; }
        public IBlessingsPull GlobalBlessingPull { get; private set; }
        

        public bool GameStarted { get; private set; }
        public event Action OnGameStart;
        public event Action Won;
        public event Action Lost;
        public event Action<int> RoundChanged;
        
        public SceneName Scene => (SceneName)SceneManager.GetActiveScene().buildIndex;
        
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
            PhaseManager.Instance.PhaseEntered.AddListener(PhaseManagerOnStateEntered);
            currentRound = 1;
            
            StartCoroutine(StartGameCoroutine());

            IEnumerator StartGameCoroutine()
            {
                yield return null;
                PhaseManager.Instance.StartGame();
                GameStarted = true;
                OnGameStart?.Invoke();
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

            if (WinAction == null)
            {
                Debug.LogError($"Нет {nameof(WinAction)}");
                return;
            }
            
            if (LoseAction == null)
            {
                Debug.LogError($"Нет {nameof(LoseAction)}");
                return;
            }
            
            GameReferee.Initialize(Player.LocalPlayer, AllPlayers
                .Select(x => x.Value)
                .Where(x => x != Player.LocalPlayer));
            
            GameReferee.Won += WinGame;
            GameReferee.Lost += LoseGame;
        }

        [EditorButton]
        public void WinGame()
        {
            WinAction.Execute();
            Won?.Invoke();
            
            // if (GameRoot.Instance != null && GameRoot.Instance.SdkAdapter != null)
            // {
            //     if (CurrentDeck != null)
            //     {
            //         foreach (var card in CurrentDeck.Cards)
            //             GameRoot.Instance.SdkAdapter.SendCardMetrica(card, Scene, true);
            //     }
            // }
        }
        
        [EditorButton]
        public void LoseGame()
        {
            LoseAction.Execute();
            Lost?.Invoke();
            
            // if (GameRoot.Instance != null && GameRoot.Instance.SdkAdapter != null)
            // {
            //     if (CurrentDeck != null)
            //     {
            //         foreach (var card in CurrentDeck.Cards)
            //             GameRoot.Instance.SdkAdapter.SendCardMetrica(card, Scene, false);
            //     }
            // }
        }

        public void PauseGame()
        {
            if (PauseInstaller.Instance)
                PauseInstaller.Pause(true);
        }

        public void ResumeGame()
        {
            if (PauseInstaller.Instance)
                PauseInstaller.Pause(false);
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (SfxManager.Instance != null) SfxManager.Instance.StopAllSounds();
            if (SpeechManager.Instance != null) SpeechManager.Instance.StopAllSounds();
            if (PhaseManager.Instance != null) PhaseManager.Instance.PhaseEntered.RemoveListener(PhaseManagerOnStateEntered);
        }
        
        private void PhaseManagerOnStateEntered(PhaseType phase)
        {
            if (phase == PhaseType.Replenish)
                CurrentRound++;
        }
    }
}