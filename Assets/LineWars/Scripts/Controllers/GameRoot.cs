using System;
using DataStructures;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public class GameRoot: DontDestroyOnLoadSingleton<GameRoot>
    {
        [SerializeField] private ScriptableDeckCardsStorage cardsDatabase;
        [SerializeField] private DecksController decksController;
        
        [Header("Providers")]
        [SerializeField] private DeckProvider deckProvider;
        [SerializeField] private UserInfoProvider userInfoProvider;
        [SerializeField] private SettingsProvider settingsProvider;
        
        public ScriptableDeckCardsStorage CardsDatabase => cardsDatabase;
        public DeckProvider DeckProvider => deckProvider;
        public DecksController DecksController => decksController;
        

        protected override void OnAwake()
        {
            ValidateFields();
            
            InitializeProviders();
            
            DecksController.Initialize(deckProvider);
        }

        private void InitializeProviders()
        {
            switch (deckProvider)
            {
                case JsonFileDeckProvider jsonFileDeckProvider:
                    jsonFileDeckProvider.Initialize(cardsDatabase.IdToCard, cardsDatabase.CardToId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            switch (userInfoProvider)
            {
                case JsonFileUserInfoProvider jsonProvider:
                    jsonProvider.Initialize();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (settingsProvider)
            {
                case JsonFileSettingsProvider jsonProvider:
                    jsonProvider.Initialize();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ValidateFields()
        {
            if (cardsDatabase == null)
                Debug.LogError($"{nameof(cardsDatabase)} is null on {name}!", gameObject);
            if (deckProvider == null)
                Debug.LogError($"{nameof(deckProvider)} is null on {name}!", gameObject);
            if (userInfoProvider == null)
                Debug.LogError($"{nameof(userInfoProvider)} is null on {name}!", gameObject);
            if (settingsProvider == null)
                Debug.LogError($"{nameof(settingsProvider)} is null on {name}!", gameObject);
            if (decksController == null)
                Debug.LogError($"{nameof(decksController)} is null on {name}!", gameObject);
        }
    }
}