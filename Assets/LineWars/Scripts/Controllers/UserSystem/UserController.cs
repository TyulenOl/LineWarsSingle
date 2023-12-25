using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public class UserController: MonoBehaviour
    {
        [SerializeField] private DefaultOpenedCards defaultOpenedCards;
        
        private IProvider<UserInfo> userInfoProvider;
        private IStorage<DeckCard> deckCardStorage;

        private UserInfo currentInfo;
        private HashSet<DeckCard> openedCardsSet;
        
        public IEnumerable<DeckCard> OpenedCards => openedCardsSet;
        
        public void Initialize(IProvider<UserInfo> provider, IStorage<DeckCard> storage)
        {
            userInfoProvider = provider;
            deckCardStorage = storage;
            currentInfo = provider.Load(0) ?? new UserInfo(){amountInGameCurrency = 10, unlockCards = storage.Keys.ToList()};

            openedCardsSet = currentInfo.unlockCards
                .Select(x => deckCardStorage.IdToValue[x])
                .Concat(defaultOpenedCards.DefaultCards.Where(storage.ValueToId.ContainsKey))
                .ToHashSet();
        }


        public void OpenCard(DeckCard deckCard)
        {
            if (openedCardsSet.Contains(deckCard))
                return;
            openedCardsSet.Add(deckCard);
            currentInfo.unlockCards.Add(deckCardStorage.ValueToId[deckCard]);
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