using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;
using System;
using System.Collections;
using LineWars.LootBoxes;

namespace LineWars.Controllers
{
    public class UserInfoController: MonoBehaviour
    {
        [SerializeField] private UserInfoPreset defaultUserInfoPreset;

#if UNITY_EDITOR
        [Header("Debug")] 
        [SerializeField] private bool debugMode;
#endif
        
        private IProvider<UserInfo> userInfoProvider;
        private IStorage<DeckCard> deckCardStorage;

        private UserInfo currentInfo;
        
        private HashSet<DeckCard> openedCardsSet;
        public IEnumerable<DeckCard> OpenedCards => openedCardsSet;

        public event Action<int> GoldChanged;
        public event Action<int> DiamondsChanged;
        public event Action<int> UserUpgradeCardsChanged;
        public event Action<int> PassingGameModesChanged; 
        
        public IReadOnlyUserInfo UserInfo => currentInfo;
        public void Initialize(IProvider<UserInfo> provider, IStorage<DeckCard> storage)
        {
            userInfoProvider = provider;
            deckCardStorage = storage;
            
            currentInfo = AssignUserInfo(provider.Load(0) ?? CreateDefaultUserInfo());
#if UNITY_EDITOR
            if (debugMode)
            {
                currentInfo.UnlockedCards = new List<int>();
                UserDiamond = 10000;
                UserGold = 10000;
            }
#endif
            InitializeDefaultCards();
            
            
            SaveCurrentUserInfo();
        }

        private void InitializeDefaultCards()
        {
            openedCardsSet = currentInfo.UnlockedCards
                .Select(x => deckCardStorage.IdToValue[x])
                .Concat(defaultUserInfoPreset.DefaultCards.Where(deckCardStorage.ValueToId.ContainsKey))
                .ToHashSet();

            currentInfo.UnlockedCards = openedCardsSet
                .Select(x => deckCardStorage.ValueToId[x])
                .ToList();

            foreach(var cardInfo in defaultUserInfoPreset.DefaultCardLevels)
            {
                var level = cardInfo.Value;
                var cardId = cardInfo.Key;

                if(!currentInfo.CardLevels.ContainsKey(cardId))
                {
                    currentInfo.CardLevels[cardId] = level;
                }
            }
        }

        private UserInfo AssignUserInfo(UserInfo userInfo)
        {
            foreach (LootBoxType boxType in Enum.GetValues(typeof(LootBoxType)))
                userInfo.LootBoxes.TryAdd(boxType, 0);
            return userInfo;
        }

        private UserInfo CreateDefaultUserInfo()
        {
            var newUserInfo = new UserInfo()
            {
                Gold = defaultUserInfoPreset.DefaultGold,
                Diamonds = defaultUserInfoPreset.DefaultDiamond,
                UnlockedCards = defaultUserInfoPreset.DefaultCards
                    .Where(deckCardStorage.ValueToId.ContainsKey)
                    .Select(x => deckCardStorage.ValueToId[x])
                    .ToList(),
                LootBoxes = Enum.GetValues(typeof(LootBoxType)).OfType<LootBoxType>()
                    .ToSerializedDictionary(x => x, x=> 0)
            };

            foreach(var pair in defaultUserInfoPreset.DefaultBoxesCount)
            {
                newUserInfo.LootBoxes[pair.Key] = pair.Value;
            }
            return newUserInfo;
        }

        public bool CardIsOpen(DeckCard card) => openedCardsSet.Contains(card);

        public void UnlockCard(int id)
        {
            UnlockCard(deckCardStorage.IdToValue[id]);
        }
        
        public void UnlockCard(DeckCard deckCard)
        {
            if (openedCardsSet.Contains(deckCard))
                return;
            openedCardsSet.Add(deckCard);
            currentInfo.UnlockedCards.Add(deckCardStorage.ValueToId[deckCard]);
            SaveCurrentUserInfo();
        }
        
        public void LockCard(int id)
        {
            LockCard(deckCardStorage.IdToValue[id]);
        }

        public void LockCard(DeckCard deckCard)
        {
            if (!openedCardsSet.Contains(deckCard))
                return;
            openedCardsSet.Remove(deckCard);
            currentInfo.UnlockedCards.Remove(deckCardStorage.ValueToId[deckCard]);
            SaveCurrentUserInfo();
        }

        public int GetCardLevel(DeckCard card)
        {
            var cardId = deckCardStorage.ValueToId[card];
            return GetCardLevel(cardId);
        }

        public int GetCardLevel(int cardId)
        {
            return currentInfo.CardLevels[cardId];
        }

        public void SetCardLevel(int cardId, int level)
        {
            if(!currentInfo.UnlockedCards.Contains(cardId))
            {
                Debug.LogError("Can't set level to locked card!");
                return;
            }
            if (level < 1)
            {
                Debug.LogError("Level can't be negative");
                return;
            }

            currentInfo.CardLevels[cardId] = level;
        }

        public void SetCardLevel(DeckCard card, int level)
        {
            var cardId = deckCardStorage.ValueToId[card];
            SetCardLevel(cardId, level);
        }

        public int UserGold
        {
            get => currentInfo.Gold;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Gold can't be less than zero!");
                if (currentInfo.Gold == value)
                    return;
                
                currentInfo.Gold = value;
                SaveCurrentUserInfo();
                GoldChanged?.Invoke(value);
            }
        }
        
        public int UserDiamond
        {
            get => currentInfo.Diamonds;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Diamonds can't be less than zero!");
                if (currentInfo.Diamonds == value)
                    return;
                
                currentInfo.Diamonds = value;
                SaveCurrentUserInfo();
                DiamondsChanged?.Invoke(value);
            }
        }

        public int UserUpgradeCards
        {
            get => currentInfo.UpgradeCards;
            set
            {
                if (value < 0)
                    throw new ArgumentException("UpgradeCards can't be less than zero!");
                if (currentInfo.UpgradeCards == value)
                    return;
                
                currentInfo.UpgradeCards = value;
                SaveCurrentUserInfo();
                UserUpgradeCardsChanged?.Invoke(value);
            }
        }

        public int PassingInfinityGameModes
        {
            get => currentInfo.PassingGameModes;
            set
            {
                if (value < 0)
                    throw new ArgumentException($"{nameof(PassingInfinityGameModes)} can't be less than zero!");
                if (currentInfo.PassingGameModes == value)
                    return;
                
                currentInfo.PassingGameModes = value;
                SaveCurrentUserInfo();
                PassingGameModesChanged?.Invoke(value);
            }
        }

        public int GetBoxes(LootBoxType boxType)
        {
            return currentInfo.LootBoxes[boxType];
        }

        public void SetBoxes(LootBoxType boxType, int value)
        {
            if (value < 0)
                throw new ArgumentException("Loot Box can't be less than zero!");
            if (currentInfo.LootBoxes[boxType].Equals(value))
                return;
            currentInfo.LootBoxes[boxType] = value;
            SaveCurrentUserInfo();
        }

        private void SaveCurrentUserInfo()
        {
            StartCoroutine(SaveCurrentUserInfoCoroutine());
        }


        private IEnumerator SaveCurrentUserInfoCoroutine()
        {
            yield return null;
#if UNITY_EDITOR
            if (!debugMode)
                userInfoProvider.Save(currentInfo, 0);
#else      
            userInfoProvider.Save(currentInfo, 0);
#endif
        }
    }
}