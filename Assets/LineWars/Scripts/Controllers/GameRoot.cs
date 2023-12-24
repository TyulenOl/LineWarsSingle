using System;
using DataStructures;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public class GameRoot : DontDestroyOnLoadSingleton<GameRoot>
    {
        [SerializeField] private ScriptableDeckCardsStorage cardsDatabase;
        [SerializeField] private DecksController decksController;
        [SerializeField] private CompaniesController companiesController;

        [Header("ProviderSettings")] 
        [SerializeField] private ProviderType providerType;

        private IProvider<Deck> deckProvider;
        private IProvider<UserInfo> userInfoProvider;
        private IProvider<Settings> settingsProvider;
        private IProvider<MissionInfo> missionInfoProvider;


        public ScriptableDeckCardsStorage CardsDatabase => cardsDatabase;
        public DecksController DecksController => decksController;
        public CompaniesController CompaniesController => companiesController;

        protected override void OnAwake()
        {
            ValidateFields();

            InitializeProviders();

            DecksController.Initialize(deckProvider);
            CompaniesController.Initialize(missionInfoProvider);
        }

        private void InitializeProviders()
        {
            switch (providerType)
            {
                case ProviderType.FileJson:
                {
                    deckProvider = new Provider<Deck>(
                        new SaverConvertDecorator<Deck, DeckInfo>(
                            new JsonFileSaver<DeckInfo>(),
                            new DeckToInfoConverter(cardsDatabase.CardToId)
                        ),
                        new DownloaderConvertDecorator<Deck, DeckInfo>(
                            new JsonFileLoader<DeckInfo>(),
                            new DeckInfoToDeckConverter(cardsDatabase.IdToCard)
                        ),
                        new AllDownloaderConvertDecorator<Deck, DeckInfo>(
                            new JsonFileAllDownloader<DeckInfo>(),
                            new DeckInfoToDeckConverter(cardsDatabase.IdToCard))
                    );

                    userInfoProvider = new Provider<UserInfo>(
                        new JsonFileSaver<UserInfo>(),
                        new JsonFileLoader<UserInfo>(),
                        new JsonFileAllDownloader<UserInfo>()
                    );

                    settingsProvider = new Provider<Settings>(
                        new JsonFileSaver<Settings>(),
                        new JsonFileLoader<Settings>(),
                        new JsonFileAllDownloader<Settings>()
                    );

                    missionInfoProvider = new Provider<MissionInfo>(
                        new JsonFileSaver<MissionInfo>(),
                        new JsonFileLoader<MissionInfo>(),
                        new JsonFileAllDownloader<MissionInfo>()
                    );
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ValidateFields()
        {
            if (cardsDatabase == null)
                Debug.LogError($"{nameof(cardsDatabase)} is null on {name}!", gameObject);
            if (decksController == null)
                Debug.LogError($"{nameof(decksController)} is null on {name}!", gameObject);
        }
    }

    public enum ProviderType
    {
        FileJson
    }
}