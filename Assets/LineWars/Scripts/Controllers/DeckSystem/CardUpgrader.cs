using LineWars.Controllers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    public class CardUpgrader : MonoBehaviour
    {
        [SerializeField] private int commonUpgradeCost;
        [SerializeField] private int epicUpgradeCost;
        private UserInfoController userInfoController;
        private IStorage<DeckCard> cardStorage;

        public void Initialize(UserInfoController userInfoController, IStorage<DeckCard> cardStorage)
        {
            this.userInfoController = userInfoController;
            this.cardStorage = cardStorage;
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
            if (card.Rarity == CardRarity.Common)
                return userInfoController.UserUpgradeCards >= commonUpgradeCost;
            return userInfoController.UserUpgradeCards >= epicUpgradeCost;
        }

        public void Upgrade(DeckCard card)
        {
            var cardId = cardStorage.ValueToId[card];
            Upgrade(cardId);
        }

        public void Upgrade(int cardId)
        {
            if (!userInfoController.UserInfo.UnlockedCards.Contains(cardId))
            {
                Debug.LogError("Can't upgrade locked card!");
                return;
            }
            var card = cardStorage.IdToValue[cardId];
            var desiredLevel = userInfoController.UserInfo.CardLevels[cardId] + 1;
            var hasLevel = desiredLevel <= card.MaxLevel;
            if(!hasLevel)
            {
                Debug.LogError("Can't upgrade to next level!");
                return;
            }
            var price = card.Rarity == CardRarity.Common ? commonUpgradeCost : epicUpgradeCost;
            if(price > userInfoController.UserUpgradeCards)
            {
                Debug.LogError("Cannot afford the upgrade");
                return;
            }
            userInfoController.SetCardLevel(card, desiredLevel);
            userInfoController.UserUpgradeCards -= price;
        }
    }
}
