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
        [SerializeField] private LayoutGroup levelLayoutGroup;
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

        [SerializeField] private TMP_Text costInfo;
        [SerializeField] private LevelCharacteristics costCharacteristics;

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
            
            
            var currentLevel = deckCard.GetLevelInfo(deckCard.Level);
            var nextLevel = deckCard.GetLevelInfo(deckCard.Level + 1);

            var currentCostInfo = currentLevel.CostProgression.GetFirstIncrement(currentLevel.Cost);
            var nextCostInfo = nextLevel?.CostProgression.GetFirstIncrement(nextLevel.Cost);

            var currentCost = currentCostInfo.CanBuy
                ? $"{currentLevel.Cost}(+{currentCostInfo.Cost})"
                : $"{currentLevel.Cost}";

            var nextCost = nextCostInfo == null
                ? null
                : nextCostInfo.CanBuy
                    ? $"{nextLevel.Cost}(+{nextLevel.Cost})"
                    : $"{nextLevel.Cost}";

            DrawCharacteristic(
                currentLevel.Unit.MaxActionPoints.ToString(),
                nextLevel?.Unit.MaxActionPoints.ToString(),
                currentAp,
                apCharacteristics);

            DrawCharacteristic(
                currentLevel.Unit.MaxHp.ToString(),
                nextLevel?.Unit.MaxHp.ToString(),
                currentHp,
                hpCharacteristics);

            DrawCharacteristic(
                currentLevel.Unit.InitialPower.ToString(),
                nextLevel?.Unit.InitialPower.ToString(),
                currentPower,
                powerCharacteristics);

            DrawCharacteristic(
                currentLevel.Unit.MaxActionPoints.ToString(),
                nextLevel?.Unit.MaxActionPoints.ToString(),
                currentAp,
                apCharacteristics);

            DrawCharacteristic(
                currentCost,
                nextCost,
                costInfo,
                costCharacteristics);

            LayoutRebuilder.ForceRebuildLayoutImmediate(levelLayoutGroup.GetComponent<RectTransform>());
        }

        private void DrawCharacteristic(
            string current,
            string next,
            TMP_Text defaultText,
            LevelCharacteristics ifLevelUpText)
        {
            if (next == null || current == next)
            {
                defaultText.gameObject.SetActive(true);
                ifLevelUpText.gameObject.SetActive(false);
                defaultText.text = current;
            }
            else
            {
                defaultText.gameObject.SetActive(false);
                ifLevelUpText.gameObject.SetActive(true);
                ifLevelUpText.Current.text = current;
                ifLevelUpText.Next.text = next;
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