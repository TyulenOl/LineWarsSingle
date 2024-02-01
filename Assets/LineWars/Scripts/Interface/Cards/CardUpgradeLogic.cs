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

        private DeckCard deckCard;

        public DeckCard DeckCard
        {
            get => deckCard;
            set
            {
                deckCard = value;
            }
        }

        [SerializeField] private TMP_Text upgradeCardsAmount;

        private void Awake()
        {
            upgradeButton.onClick.AddListener(OnClick);
        }

        private void OnEnable()
        {
            upgradeCardsAmount.text = GetAmountText();
            if (deckCard == null)
            {
                upgradeButton.interactable = false;
                return;
            }
            upgradeButton.interactable = GameRoot.Instance.CardUpgrader.CanUpgrade(deckCard);
        }

        private void OnClick()
        {
        }

        private string GetAmountText()
        {
            return $"{GameRoot.Instance.UserController.UserUpgradeCards}/{1}";
        }
    }
}