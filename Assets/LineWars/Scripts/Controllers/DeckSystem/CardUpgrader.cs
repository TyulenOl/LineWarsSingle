using LineWars.Controllers;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    public class CardUpgrader : MonoBehaviour
    {
        private UserInfoController userInfoController;
        private IStorage<int, DeckCard> cardStorage;

        public void Initialize(UserInfoController userInfoController, IStorage<int, DeckCard> cardStorage)
        {
            this.userInfoController = userInfoController;
            this.cardStorage = cardStorage;
            SetInitialCardLevel();
        }
        private void SetInitialCardLevel()
        {
            foreach(var card in cardStorage.Values)
            {
                var level = userInfoController.GetCardLevel(card);
                card.Level = level;
            }
        }

        public bool CanUpgrade(DeckCard card)
        {
            var cardId = cardStorage.ValueToId[card];
            return CanUpgrade(cardId);
        }

        public bool CanUpgrade(int cardId)
        {
            if (!userInfoController.UserInfo.UnlockedCards.Contains(cardId))
                return false;
            var card = cardStorage.IdToValue[cardId];   
            var desiredLevel = userInfoController.UserInfo.CardLevels[cardId] + 1;
            var hasLevel = desiredLevel <= card.MaxLevel;
            if (!hasLevel)
                return false;
            var levelInfo = card.GetLevelInfo(desiredLevel);
            return userInfoController.UserUpgradeCards >= levelInfo.CostToUpgrade;
        }

        public int GetUpgradePrice(DeckCard deckCard)
        {
            if (!cardStorage.TryGetKey(deckCard, out int cardId))
                return -1;
            if (!userInfoController.CardIsOpen(deckCard))
                return -1;
                
            var desiredLevel = userInfoController.UserInfo.CardLevels[cardId] + 1;
            var hasLevel = desiredLevel <= deckCard.MaxLevel;
            if (!hasLevel)
                return -1;
            
            var levelInfo = deckCard.GetLevelInfo(desiredLevel);
            return levelInfo.CostToUpgrade;
        }
        
        public void Upgrade(DeckCard card)
        {
            var cardId = cardStorage.ValueToId[card];
            Upgrade(cardId);
        }

        public void Upgrade(int cardId)
        {
            var card = cardStorage.IdToValue[cardId];
            var desiredLevel = userInfoController.UserInfo.CardLevels[cardId] + 1;
            var hasLevel = desiredLevel <= card.MaxLevel;
            if(!hasLevel)
            {
                Debug.LogError("Can't upgrade to next level!");
                return;
            }
            var levelInfo = card.GetLevelInfo(desiredLevel);
            var price = levelInfo.CostToUpgrade;
            if(price > userInfoController.UserUpgradeCards)
            {
                Debug.LogError("Cannot afford the upgrade");
                return;
            }

            userInfoController.SetCardLevel(card, desiredLevel);
            userInfoController.UserUpgradeCards -= price;
            card.Level++;
        }
    }
}
