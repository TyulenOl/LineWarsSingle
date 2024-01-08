using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public class UserInfoController: MonoBehaviour
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
            
            openedCardsSet = currentInfo.UnlockedCards
                .Select(x => deckCardStorage.IdToValue[x])
                .Concat(userInfoPreset.DefaultCards.Where(storage.ValueToId.ContainsKey))
                .ToHashSet();
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
                    .ToList()
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

        public void ChangeGold(int value)
        {
            currentInfo.Gold += value;
        }

        public void ChangeDiamond(int value)
        {
            currentInfo.Diamonds += value;
        }

        public void ChangeUpgradeCards(int value)
        {
            currentInfo.UpgradeCards += value;
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