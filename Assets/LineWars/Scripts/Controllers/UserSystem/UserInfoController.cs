using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;
using System;
using LineWars.LootBoxes;

namespace LineWars.Controllers
{
    public class UserInfoController: MonoBehaviour
    {
        [SerializeField] private UserInfoPreset userInfoPreset;

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
        
        public void Initialize(IProvider<UserInfo> provider, IStorage<DeckCard> storage)
        {
            userInfoProvider = provider;
            deckCardStorage = storage;

            currentInfo = AssignUserInfo(provider.Load(0) ?? CreateDefaultUserInfo());
#if UNITY_EDITOR
            if (debugMode)
            {
                currentInfo.UnlockedCards = new List<int>();
                UserDiamond = 100;
                UserGold = 100;
            }    
#endif
            openedCardsSet = currentInfo.UnlockedCards
                .Select(x => deckCardStorage.IdToValue[x])
                .Concat(userInfoPreset.DefaultCards.Where(storage.ValueToId.ContainsKey))
                .ToHashSet();
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
                Gold = userInfoPreset.DefaultGold,
                Diamonds = userInfoPreset.DefaultDiamond,
                UnlockedCards = userInfoPreset.DefaultCards
                    .Where(deckCardStorage.ValueToId.ContainsKey)
                    .Select(x => deckCardStorage.ValueToId[x])
                    .ToList(),
                LootBoxes = Enum.GetValues(typeof(LootBoxType)).OfType<LootBoxType>()
                    .ToSerializedDictionary(x => x, x=> 0)
            };

            foreach(var pair in userInfoPreset.DefaultBoxesCount)
            {
                newUserInfo.LootBoxes[pair.Key] = pair.Value;
            }
            return newUserInfo;
        }

        public bool CardIsOpen(DeckCard card) => openedCardsSet.Contains(card);

        public void UnlockCard(int id) //не очень оптимизированно?
        {
            UnlockCard(deckCardStorage.IdToValue[id]);
        }
        
        public void UnlockCard(DeckCard deckCard)
        {
            if (openedCardsSet.Contains(deckCard))
                return;
            openedCardsSet.Add(deckCard);
            currentInfo.UnlockedCards.Add(deckCardStorage.ValueToId[deckCard]);
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
        }

        public int UserGold
        {
            get => currentInfo.Gold;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Gold can't be less than zero!");
                currentInfo.Gold = value;
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
                currentInfo.Diamonds = value;
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
                currentInfo.UpgradeCards = value;
                UserUpgradeCardsChanged?.Invoke(value);
            }
        }

        public int PassingGameModes
        {
            get => currentInfo.PassingGameModes;
            set
            {
                if (value < 0)
                    throw new ArgumentException("NumberSuccessfullyCompletedEndlessMode can't be less than zero!");
                currentInfo.PassingGameModes = value;
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
            currentInfo.LootBoxes[boxType] = value;

        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                userInfoProvider.Save(currentInfo, 0);
            }
        }
    }
}