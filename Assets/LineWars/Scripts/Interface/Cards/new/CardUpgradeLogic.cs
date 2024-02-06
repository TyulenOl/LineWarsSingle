using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class CardUpgradeLogic : MonoBehaviour
    {
        [SerializeField] private Button upgradeButton;
        [SerializeField] private TMP_Text upgradeCardsAmount;
        [SerializeField] private Image upgradeCardsIcon;
        [SerializeField] private TMP_Text levelText;
        private DeckCard deckCard;

        private static CardUpgrader CardUpgrader => GameRoot.Instance.CardUpgrader;
        private static UserInfoController UserController => GameRoot.Instance.UserController;

        public DeckCard DeckCard
        {
            get => deckCard;
            set
            {
                deckCard = value;
                Redraw(value);
            }
        }

        private void Start()
        {
            upgradeButton.onClick.AddListener(UpgradeCard);
        }
        
        private void OnDestroy()
        {
            if (upgradeButton != null)
                upgradeButton.onClick.RemoveListener(UpgradeCard);
        }

        private void Redraw(DeckCard deckCard)
        {
            if (deckCard == null)
                return;
            var canUpgrade = CardUpgrader.CanUpgrade(deckCard);
            
            upgradeButton.interactable = canUpgrade;

            var costToUpgrade = CardUpgrader.GetUpgradePrice(deckCard);
            if (costToUpgrade != -1)
            {
                upgradeCardsAmount.text = $"{UserController.UserUpgradeCards}/{costToUpgrade}";
            }
            else
            {
                upgradeCardsAmount.text = $"{UserController.UserUpgradeCards}/-";
            }

            levelText.text = $"{UserController.GetCardLevel(deckCard) + 1}";
        }

        private void UpgradeCard()
        {
            if (DeckCard == null)
                throw new InvalidOperationException();
            var canUpgrade = CardUpgrader.CanUpgrade(DeckCard);
            if (!canUpgrade)
                throw new InvalidOperationException();

            CardUpgrader.Upgrade(DeckCard);
            Redraw(DeckCard);
        }
    }
}