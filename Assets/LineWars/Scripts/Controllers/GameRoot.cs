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
        
        [Header("Providers")]
        [SerializeField] private YandexGameProvider yandexGameProvider;
        
        [Header("RewardControllers")]
        [SerializeField] private YandexGameRewardController yandexGameRewardController;

        [Space]
        [SerializeField] private Platform platform;
        
        private IProvider<Deck> deckProvider;
        private IProvider<MissionInfo> missionInfoProvider;
        private IProvider<UserInfo> userInfoProvider;
        private IGetter<DateTime> timeGetter;

        public IStorage<int, DeckCard> CardsDatabase => cardsDatabase;
        public IStorage<int, MissionData> MissionsStorage => missionsStorage;
        public IStorage<BlessingId, BaseBlessing> BlessingStorage => blessingStorage;

        public DecksController DecksController => decksController;
        public CompaniesController CompaniesController => companiesController;
        public UserInfoController UserController => userController;
        public LootBoxController LootBoxController => lootBoxController;
        public RewardControllerBase RewardController { get; private set; }

        public BlessingsController BlessingsController => blessingsController;
        public Store Store => store;
        public CardUpgrader CardUpgrader => cardUpgrader;

        public DrawHelperInstance DrawHelper => drawHelper;
        
        public bool GameReady { get; private set; }
        public event Action OnGameReady;

        protected override void OnAwake()
        {
            GameVariables.Initialize();
            ValidateFields();
            timeGetter = gameObject.AddComponent<GetWorldTime>();
            
            InitializeProviders(StartGame);
        }

        private void StartGame()
        {
            CompaniesController.Initialize(missionInfoProvider, missionsStorage);
            UserController.Initialize(userInfoProvider, cardsDatabase);
            DecksController.Initialize(deckProvider, UserController);
            Store.Initialize(timeGetter, CardsDatabase, BlessingStorage, UserController);
            BlessingsController.Initialize(UserController, UserController, BlessingStorage);
            CardUpgrader.Initialize(userController, cardsDatabase);
            InitializeLootBoxController();

            RewardController = platform switch
            {
                Platform.Local => gameObject.AddComponent<RewardControllerBase>(),
                Platform.YandexGame => yandexGameRewardController,
                _ => throw new ArgumentOutOfRangeException()
            };

            GameReady = true;
            OnGameReady?.Invoke();
        }

        private void InitializeProviders(Action initialize)
        {
            switch (platform)
            {
                case Platform.Local:
                {
                    InitializeJsonProviders(initialize);
                    break;
                }
                case Platform.YandexGame:
                {
                    InitializeYandexGameProvider(initialize);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void InitializeJsonProviders(Action initialize)
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

            missionInfoProvider = new Provider<MissionInfo>(
                new JsonFileSaver<MissionInfo>(),
                new JsonFileLoader<MissionInfo>(),
                new JsonFileAllDownloader<MissionInfo>()
            );
            
            initialize.Invoke();
        }

        private void InitializeYandexGameProvider(Action initialize)
        {
            deckProvider = new Provider<Deck>(
                new SaverConvertDecorator<Deck, DeckInfo>(
                    yandexGameProvider,
                    new DeckToInfoConverter(cardsDatabase.ValueToId)
                ),
                new DownloaderConvertDecorator<Deck, DeckInfo>(
                    yandexGameProvider,
                    new DeckInfoToDeckConverter(cardsDatabase.IdToValue)
                ),
                new AllDownloaderConvertDecorator<Deck, DeckInfo>(
                    yandexGameProvider,
                    new DeckInfoToDeckConverter(cardsDatabase.IdToValue))
            );


            userInfoProvider = yandexGameProvider;
            missionInfoProvider = yandexGameProvider;

            yandexGameProvider.FinishLoad += initialize;
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

    public enum Platform
    {
        Local,
        YandexGame
    }
}