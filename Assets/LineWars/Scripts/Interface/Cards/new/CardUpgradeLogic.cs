using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Interface;
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

        [SerializeField] private TMP_Text currentAp;
        [SerializeField] private LevelCharacteristics apCharacteristics;
        
        [SerializeField] private TMP_Text currentHp;
        [SerializeField] private LevelCharacteristics hpCharacteristics;
        
        [SerializeField] private TMP_Text currentPower;
        [SerializeField] private LevelCharacteristics powerCharacteristics;
        
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


            if (canUpgrade)
            {
                var currentLevel = deckCard.GetLevelInfo(deckCard.Level);
                var nextLevel = deckCard.GetLevelInfo(deckCard.Level + 1);
                currentAp.gameObject.SetActive(false);
                currentHp.gameObject.SetActive(false);
                currentPower.gameObject.SetActive(false);
                
                apCharacteristics.gameObject.SetActive(true);
                hpCharacteristics.gameObject.SetActive(true);
                powerCharacteristics.gameObject.SetActive(true);

                apCharacteristics.Current.text = currentLevel.Unit.MaxActionPoints.ToString();
                hpCharacteristics.Current.text = currentLevel.Unit.MaxHp.ToString();
                powerCharacteristics.Current.text = currentLevel.Unit.InitialPower.ToString();
                
                apCharacteristics.Next.text = nextLevel.Unit.MaxActionPoints.ToString();
                hpCharacteristics.Next.text = nextLevel.Unit.MaxHp.ToString();
                powerCharacteristics.Next.text = nextLevel.Unit.InitialPower.ToString();
            }
            else
            {
                currentAp.gameObject.SetActive(true);
                currentHp.gameObject.SetActive(true);
                currentPower.gameObject.SetActive(true);
                
                apCharacteristics.gameObject.SetActive(false);
                hpCharacteristics.gameObject.SetActive(false);
                powerCharacteristics.gameObject.SetActive(false);
            }
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