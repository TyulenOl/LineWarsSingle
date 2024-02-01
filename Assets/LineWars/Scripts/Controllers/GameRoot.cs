using System;
using DataStructures;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public class GameRoot : DontDestroyOnLoadSingleton<GameRoot>
    {
        [Header("Draw")] 
        [SerializeField] private DrawHelperInstance drawHelper;
        
        [Header("Storages")]
        [SerializeField] private DeckCardsScriptableStorage cardsDatabase;
        [SerializeField] private MissionsScriptableStorage missionsStorage;
        [SerializeField] private BlessingStorage blessingStorage;

        [Header("Controllers")]
        [SerializeField] private DecksController decksController;
        [SerializeField] private CompaniesController companiesController;
        [SerializeField] private UserInfoController userController;
        [SerializeField] private LootBoxController lootBoxController;
        [SerializeField] private BlessingsController blessingsController;
        [SerializeField] private Store store;
        [SerializeField] private CardUpgrader cardUpgrader;

        [Header("ProviderSettings")]
        [SerializeField] private ProviderType providerType;

        private CardLevelsStorage cardsLevelStorage;
        private IProvider<Deck> deckProvider;
        private IProvider<MissionInfo> missionInfoProvider;
        private IProvider<UserInfo> userInfoProvider;
        private IProvider<Settings> settingsProvider;
        private IGetter<DateTime> timeGetter;

        public IStorage<int, DeckCard> CardsDatabase => cardsDatabase;
        public IStorage<int, MissionData> MissionsStorage => missionsStorage;
        public IStorage<BlessingId, BaseBlessing> BlessingStorage => blessingStorage;
        public CardLevelsStorage CardsLevelDatabase => cardsLevelStorage;

        public DecksController DecksController => decksController;
        public CompaniesController CompaniesController => companiesController;
        public UserInfoController UserController => userController;
        public LootBoxController LootBoxController => lootBoxController;

        public BlessingsController BlessingsController => blessingsController;
        public Store Store => store;
        public CardUpgrader CardUpgrader => cardUpgrader;

        public DrawHelperInstance DrawHelper => drawHelper;

        protected override void OnAwake()
        {
            GameVariables.Initialize();

            ValidateFields();

            InitializeProviders();

            CompaniesController.Initialize(missionInfoProvider, missionsStorage);
            UserController.Initialize(userInfoProvider, cardsDatabase);
            DecksController.Initialize(deckProvider, UserController);
            Store.Initialize(timeGetter, CardsDatabase, BlessingStorage, UserController);
            cardsLevelStorage = new CardLevelsStorage(CardsDatabase, UserController.UserInfo);
            BlessingsController.Initialize(UserController, UserController, BlessingStorage);
            CardUpgrader.Initialize(userController, cardsDatabase);
            InitializeLootBoxController();
        }

        private void InitializeProviders()
        {
            timeGetter = gameObject.AddComponent<GetWorldTime>();
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
            var dropConverter = new DuplicateEreaserDropConverter(userController.UserInfo, cardsDatabase);
            lootBoxController.Initialize(
                userController.UserInfo, 
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