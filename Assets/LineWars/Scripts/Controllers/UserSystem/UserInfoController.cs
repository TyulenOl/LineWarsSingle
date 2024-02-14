using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;
using System;
using System.Collections;
using AYellowpaper.SerializedCollections;

namespace LineWars.Controllers
{
    public class UserInfoController: MonoBehaviour, IBlessingsPull, IBlessingSelector
    {
        [SerializeField] private UserInfoPreset defaultUserInfoPreset;

#if UNITY_EDITOR
        [Header("Debug")] 
        [SerializeField] private bool debugMode;
#endif
        
        private IProvider<UserInfo> userInfoProvider;
        private IStorage<int, DeckCard> deckCardStorage;
        private IStorage<BlessingId, BaseBlessing> blessingStorage;

        private UserInfo currentInfo;
        
        private HashSet<DeckCard> openedCardsSet;
        public IEnumerable<DeckCard> OpenedCards => openedCardsSet;

        public event Action<int> GoldChanged;
        public event Action<int> DiamondsChanged;
        public event Action<int> UserUpgradeCardsChanged;
        public event Action<int> PassingGameModesChanged;
        public event Action<bool> LockAdChanged; 
        public event Action<BlessingId, int> BlessingCountChanged;
        public event Action<BlessingId, int> SelectedBlessingIdChanged;
        public event Action<int> TotalSelectionCountChanged;


        public IReadOnlyUserInfo UserInfo => currentInfo;
        public IBlessingsPull GlobalBlessingsPull => this;
        public void Initialize(
            IProvider<UserInfo> provider, 
            IStorage<int, DeckCard> deckCardStorage,
            IStorage<BlessingId, BaseBlessing> blessingStorage)
        {
            userInfoProvider = provider;
            this.deckCardStorage = deckCardStorage;
            this.blessingStorage = blessingStorage; 
            
            currentInfo = AssignUserInfo(provider.Load(0) ?? CreateDefaultUserInfo());
            
#if UNITY_EDITOR
            if (debugMode)
            {
                currentInfo.UnlockedCards = new List<int>();
                UserDiamond = 10000;
                UserGold = 10000;
            }
#endif
            DeleteNullCards();
            InitializeDefaultCards();
            SaveCurrentUserInfo();
        }
        private void DeleteNullCards()
        {
            currentInfo.UnlockedCards = currentInfo.UnlockedCards
                .Where(id => deckCardStorage.ContainsKey(id))
                .ToList();
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
            
            foreach(var (cardId, level) in defaultUserInfoPreset.DefaultCardLevels)
                currentInfo.CardLevels.TryAdd(cardId, level);
        }
        
        private UserInfo AssignUserInfo(UserInfo userInfo)
        {
            userInfo.LootBoxes ??= new SerializedDictionary<LootBoxType, int>();
            userInfo.CardLevels ??= new SerializedDictionary<int, int>();
            userInfo.Blessings ??= new SerializedDictionary<BlessingId, int>();
            userInfo.SelectedBlessings ??= new List<BlessingId>();
            userInfo.UnlockedCards ??= new List<int>();
            userInfo.UsedPromoCodes ??= new List<string>();
            
            foreach (LootBoxType boxType in Enum.GetValues(typeof(LootBoxType)))
                userInfo.LootBoxes.TryAdd(boxType, 0);
            foreach (var cardId in deckCardStorage.Keys)
                userInfo.CardLevels.TryAdd(cardId, 0);
            
            userInfo = TryAddDefaultBlessings(userInfo);
            
            return userInfo;
        }
        
        private UserInfo TryAddDefaultBlessings(UserInfo userInfo)
        {
            if (userInfo.DefaultBlessingsIsAdded) 
                return userInfo;
            
            foreach (var (key, value) in defaultUserInfoPreset.DefaultBlessingsCount)
            {
                if (!blessingStorage.TryGetKey(key, out var id))
                {
                    DebugUtility.LogError($"Благословление {key} не содержится в базе данный!");
                    continue;
                }
                
                userInfo.Blessings.TryAdd(id, 0);
                userInfo.Blessings[id] += value;
            }
            userInfo.DefaultBlessingsIsAdded = true;

            return userInfo;
        }

        private UserInfo CreateDefaultUserInfo()
        {
            return new UserInfo()
            {
                Gold = defaultUserInfoPreset.DefaultGold,
                Diamonds = defaultUserInfoPreset.DefaultDiamond,
                UnlockedCards = defaultUserInfoPreset.DefaultCards
                    .Where(deckCardStorage.ValueToId.ContainsKey)
                    .Select(x => deckCardStorage.ValueToId[x])
                    .ToList(),
                LootBoxes = defaultUserInfoPreset.DefaultBoxesCount
                    .ToSerializedDictionary(x => x.Key, x => x.Value),
                DefaultBlessingsIsAdded = true,
                Blessings = defaultUserInfoPreset.DefaultBlessingsCount
                    .Where(pair => blessingStorage.ContainsValue(pair.Key))
                    .ToSerializedDictionary(pair => blessingStorage.ValueToId[pair.Key], pair => pair.Value),
                SelectedBlessings = defaultUserInfoPreset.DefaultSelectedBlessings
                    .Where(blessing => blessingStorage.ContainsValue(blessing))
                    .Where(blessing => defaultUserInfoPreset.DefaultBlessingsCount.ContainsKey(blessing))
                    .Select(blessing => blessingStorage.ValueToId[blessing])
                    .ToList(),
                CardLevels = defaultUserInfoPreset.DefaultCards
                    .Where(deckCardStorage.ValueToId.ContainsKey)
                    .Select(x => deckCardStorage.ValueToId[x])
                    .ToSerializedDictionary((cardId) => cardId, (_) => 0)
            };
        }

        public bool CardIsOpen(DeckCard card) => openedCardsSet.Contains(card);

        public bool CardIsOpen(int cardId)
        {
            var card = deckCardStorage.IdToValue[cardId];
            return CardIsOpen(card);
        }

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
            SaveCurrentUserInfo();
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

        public bool LockAd
        {
            get => currentInfo.LockAd;
            set
            {
                var prevValue = currentInfo.LockAd;
                currentInfo.LockAd = value;
                if (prevValue != value)
                {
                    SaveCurrentUserInfo();
                    LockAdChanged?.Invoke(value);
                }
            }
        }

        #region BlessingPullImplimintation
        int IBlessingsPull.this[BlessingId id]
        {
            get => currentInfo.Blessings.TryGetValue(id, out var value) ? value : 0;
            set
            {
                if (value < 0)
                    throw new ArgumentException($"{nameof(GlobalBlessingsPull)} can't be less than zero!");
                    
                
                if (currentInfo.Blessings.TryGetValue(id, out var currentCount))
                {
                    if (value == 0 && currentCount == 0)
                    {
                        currentInfo.Blessings.Remove(id);
                        SaveCurrentUserInfo();
                        return;
                    }
                    if (value == currentCount)
                        return;
                    currentInfo.Blessings[id] = value;
                    BlessingCountChanged?.Invoke(id, value);
                    SaveCurrentUserInfo();
                }
                else
                {
                    if (value == 0)
                        return;
                    currentInfo.Blessings[id] = value;
                    BlessingCountChanged?.Invoke(id, value);
                    SaveCurrentUserInfo();
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GlobalBlessingsPull.GetEnumerator();
        }

        IEnumerator<(BlessingId, int)> IEnumerable<(BlessingId, int)>.GetEnumerator()
        {
            foreach (var (key, value) in currentInfo.Blessings)
            {
                yield return (key, value);
            }
        }

        bool IBlessingsPull.TryGetCount(BlessingId id, out int count)
        {
            return currentInfo.Blessings.TryGetValue(id, out count);
        }

        bool IBlessingsPull.ContainsId(BlessingId id)
        {
            return currentInfo.Blessings.ContainsKey(id);
        }
        #endregion

        #region BlessingSelectorImplimitation
        IEnumerator<BlessingId> IEnumerable<BlessingId>.GetEnumerator()
        {
            return currentInfo.SelectedBlessings.GetEnumerator();
        }

        int IReadOnlyCollection<BlessingId>.Count => currentInfo.SelectedBlessings.Count;

        int IBlessingSelector.Count
        {
            get => currentInfo.SelectedBlessings.Count;
            set
            {
                if (value < 0)
                    throw new ArgumentException($"{nameof(IBlessingSelector.Count)} can't be less than zero!");
                var currentCount = currentInfo.SelectedBlessings.Count;
                if (value == currentCount)
                    return;
                
                if (value > currentCount)
                {
                    var diff = value - currentCount;
                    for (var i = 0; i < diff; i++)
                        currentInfo.SelectedBlessings.Add(BlessingId.Null);
                }
                else if (value < currentCount)
                {
                    var diff = currentCount - value;
                    for (var i = 0; i < diff; i++)
                        currentInfo.SelectedBlessings.RemoveAt(currentInfo.SelectedBlessings.Count-1);
                }
                
                TotalSelectionCountChanged?.Invoke(value);
            }
        }


        BlessingId IBlessingSelector.this[int index]
        {
            get => currentInfo.SelectedBlessings[index];
            set
            {
                if (value == currentInfo.SelectedBlessings[index])
                    return;
                currentInfo.SelectedBlessings[index] = value;
                SelectedBlessingIdChanged?.Invoke(value, index);
                SaveCurrentUserInfo();
            }
        }

        bool IBlessingSelector.CanSetValue(int index, BlessingId blessingId)
        {
            return index >= 0 && index < currentInfo.SelectedBlessings.Count;
        }

        #endregion
        

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

        public bool PromoCodeIsUsed(string promoCode)
        {
            return currentInfo.UsedPromoCodes.Contains(promoCode);
        }
        
        public IReadOnlyList<string> GetUsedPromoCodes()
        {
            return currentInfo.UsedPromoCodes;
        }

        public void UsePromoCode(string promoCode)
        {
            if (currentInfo.UsedPromoCodes.Contains(promoCode))
                return;
            
            currentInfo.UsedPromoCodes.Add(promoCode);
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