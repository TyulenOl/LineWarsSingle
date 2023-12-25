using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public class UserController: MonoBehaviour
    {
        [SerializeField] private UserInfoPreset userInfoPreset;
        
        private IProvider<UserInfo> userInfoProvider;
        private IStorage<DeckCard> deckCardStorage;

        private UserInfo currentInfo;
        private HashSet<DeckCard> openedCardsSet;
        public IEnumerable<DeckCard> OpenedCards => openedCardsSet;
        
        public void Initialize(IProvider<UserInfo> provider, IStorage<DeckCard> storage)
        {
            userInfoProvider = provider;
            deckCardStorage = storage;

            currentInfo = provider.Load(0) ?? CreateDefaultUserInfo();
            
            openedCardsSet = currentInfo.unlockCards
                .Select(x => deckCardStorage.IdToValue[x])
                .Concat(userInfoPreset.DefaultCards.Where(storage.ValueToId.ContainsKey))
                .ToHashSet();
        }

        private UserInfo CreateDefaultUserInfo()
        {
            return new UserInfo()
            {
                amountInGameCurrency = userInfoPreset.DefaultMoney,
                unlockCards = userInfoPreset.DefaultCards
                    .Where(deckCardStorage.ValueToId.ContainsKey)
                    .Select(x => deckCardStorage.ValueToId[x])
                    .ToList()
            };
        }

        public bool CardIsOpen(DeckCard card) => openedCardsSet.Contains(card);
        public void OpenCard(DeckCard deckCard)
        {
            if (openedCardsSet.Contains(deckCard))
                return;
            openedCardsSet.Add(deckCard);
            currentInfo.unlockCards.Add(deckCardStorage.ValueToId[deckCard]);
        }
        
        public void CloseCard(DeckCard deckCard)
        {
            if (!openedCardsSet.Contains(deckCard))
                return;
            openedCardsSet.Remove(deckCard);
            currentInfo.unlockCards.Remove(deckCardStorage.ValueToId[deckCard]);
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