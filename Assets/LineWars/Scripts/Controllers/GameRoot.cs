using System;
using DataStructures;
using LineWars.Model;
using LineWars.LootBoxes;
using UnityEngine;
using LineWars.Store;

namespace LineWars.Controllers
{
    public class GameRoot : DontDestroyOnLoadSingleton<GameRoot>
    {
        [Header("Storages")]
        [SerializeField] private DeckCardsScriptableStorage cardsDatabase;
        [SerializeField] private MissionsScriptableStorage missionsStorage;

        [Header("Controllers")]
        [SerializeField] private DecksController decksController;
        [SerializeField] private CompaniesController companiesController;
        [SerializeField] private UserInfoController userController;
        [SerializeField] private LootBoxController lootBoxController;
        [SerializeField] private CardStore cardStore;

        [Header("ProviderSettings")]
        [SerializeField] private ProviderType providerType;


        private IProvider<Deck> deckProvider;
        private IProvider<MissionInfo> missionInfoProvider;
        private IProvider<UserInfo> userInfoProvider;
        private IProvider<Settings> settingsProvider;
        private IGetter<DateTime> timeGetter;

        public IStorage<DeckCard> CardsDatabase => cardsDatabase;
        public IStorage<MissionData> MissionsStorage => missionsStorage;

        public DecksController DecksController => decksController;
        public CompaniesController CompaniesController => companiesController;
        public UserInfoController UserController => userController;
        public LootBoxController LootBoxController => lootBoxController;
        public CardStore CardStore => cardStore;

        protected override void OnAwake()
        {
            GameVariables.Initialize();

            ValidateFields();

            InitializeProviders();

            InitializeLootBoxController();
            DecksController.Initialize(deckProvider);
            CompaniesController.Initialize(missionInfoProvider, missionsStorage);
            UserController.Initialize(userInfoProvider, cardsDatabase);
            CardStore.Initialize(timeGetter, CardsDatabase, UserController);

        }

        private void InitializeProviders()
        {
            timeGetter = new GetWorldTime();
            switch (providerType)
            {
                case ProviderType.FileJson:
                    {
                        InitializeJsonProviders();
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void InitializeJsonProviders()
        {
            deckProvider = new Provider<Deck>(
                new SaverConvertDecorator<Deck, DeckInfo>(
                    new JsonFileSaver<DeckInfo>(),
                    new DeckToInfoConverter(cardsDatabase.ValueToId)
                ),
                new DownloaderConvertDecorator<Deck, DeckInfo>(
                    new JsonFileLoader<DeckInfo>(),
                    new DeckInfoToDeckConverter(cardsDatabase.IdToValue)
                ),
                new AllDownloaderConvertDecorator<Deck, DeckInfo>(
                    new JsonFileAllDownloader<DeckInfo>(),
                    new DeckInfoToDeckConverter(cardsDatabase.IdToValue))
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
        }

        private void InitializeLootBoxController()
        {
            var lootBoxOpenerFabric = new ClientLootBoxOpenerFabric(cardsDatabase);
            var dropConverter = new DuplicateEreaserDropConverter(userInfoProvider, cardsDatabase);
            lootBoxController.Initialize(
                userInfoProvider, 
                lootBoxOpenerFabric, 
                dropConverter, 
                userController);

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