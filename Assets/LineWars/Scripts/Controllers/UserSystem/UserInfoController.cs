﻿using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;
using System;
using System.Collections;
using LineWars.LootBoxes;
using Object = System.Object;

namespace LineWars.Controllers
{
    public class UserInfoController: MonoBehaviour, IBlessingsPull
    {
        [SerializeField] private UserInfoPreset userInfoPreset;

#if UNITY_EDITOR
        [Header("Debug")] 
        [SerializeField] private bool debugMode;
#endif
        
        private IProvider<UserInfo> userInfoProvider;
        private IStorage<int, DeckCard> deckCardStorage;

        private UserInfo currentInfo;
        
        private HashSet<DeckCard> openedCardsSet;
        public IEnumerable<DeckCard> OpenedCards => openedCardsSet;

        public event Action<int> GoldChanged;
        public event Action<int> DiamondsChanged;
        public event Action<int> UserUpgradeCardsChanged;
        public event Action<int> PassingGameModesChanged;
        public event Action<BlessingId, int> BlessingCountChanged;

        public IReadOnlyUserInfo UserInfo => currentInfo;
        public IBlessingsPull BlessingsCount => this;
        public void Initialize(IProvider<UserInfo> provider, IStorage<int, DeckCard> storage)
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
            openedCardsSet = currentInfo.UnlockedCards
                .Select(x => deckCardStorage.IdToValue[x])
                .Concat(userInfoPreset.DefaultCards.Where(storage.ValueToId.ContainsKey))
                .ToHashSet();
            
            currentInfo.UnlockedCards = openedCardsSet
                .Select(x => deckCardStorage.ValueToId[x])
                .ToList();

            if (!currentInfo.DefaultBlessingsIsAdded)
            {
                foreach (var (key, value) in userInfoPreset.DefaultBlessingsCount)
                {
                    currentInfo.Blessings.TryAdd(key, 0);
                    currentInfo.Blessings[key] += value;
                }
                currentInfo.DefaultBlessingsIsAdded = true;
            }
            
            SaveCurrentUserInfo();
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
                    .ToSerializedDictionary(x => x, x=> 0),
                DefaultBlessingsIsAdded = true,
                Blessings = userInfoPreset.DefaultBlessingsCount
                    .ToSerializedDictionary(x=> x.Key, x => x.Value)
            };

            foreach(var pair in userInfoPreset.DefaultBoxesCount)
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

        int IBlessingsPull.this[BlessingId blessingId]
        {
            get => currentInfo.Blessings.TryGetValue(blessingId, out var value) ? value : 0;
            set
            {
                if (value < 0)
                    throw new ArgumentException($"{nameof(BlessingsCount)} can't be less than zero!");
                    
                
                if (currentInfo.Blessings.TryGetValue(blessingId, out var currentCount))
                {
                    if (value == 0 && currentCount == 0)
                    {
                        currentInfo.Blessings.Remove(blessingId);
                        SaveCurrentUserInfo();
                        return;
                    }
                    if (value == currentCount)
                        return;
                    currentInfo.Blessings[blessingId] = value;
                    BlessingCountChanged?.Invoke(blessingId, value);
                    SaveCurrentUserInfo();
                }
                else
                {
                    if (value == 0)
                        return;
                    currentInfo.Blessings[blessingId] = value;
                    BlessingCountChanged?.Invoke(blessingId, value);
                    SaveCurrentUserInfo();
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return BlessingsCount.GetEnumerator();
        }

        IEnumerator<(BlessingId, int)> IEnumerable<(BlessingId, int)>.GetEnumerator()
        {
            foreach (var (key, value) in currentInfo.Blessings)
            {
                yield return (key, value);
            }
        }

        public bool TryGetValue(BlessingId id, out int count)
        {
            return currentInfo.Blessings.TryGetValue(id, out count);
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