using System;
using LineWars.Controllers;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class CardBuyPresetDrawer : MonoBehaviour
    {
        [SerializeField] private TMP_Text cost;
        [SerializeField] private Image image;
        [SerializeField] private Image moneyImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Button button;
        [SerializeField] private Image ifChosenPanel;
        [SerializeField] private Image ifCannotBuyImage;

        private bool isAvailable;

        public Button Button => button;

        private DeckCard deckCard;

        public bool IsAvailable => isAvailable;

        public DeckCard DeckCard
        {
            get => deckCard;
            set
            {
                deckCard = value;
                Init();
            }
        }

        private void Start()
        {
            PhaseManager.Instance.PhaseEntered.AddListener(OnPhaseEntered);
            CommandsManager.Instance.BuyNeedRedraw += (_) => SetAvailable();
        }

        private void OnPhaseEntered(PhaseType phaseTypeNew)
        {
            if (phaseTypeNew != PhaseType.Buy) return;
            SetAvailable();
        }

        private void Init()
        {
            image.sprite = deckCard.Image;
            cost.text = Player.LocalPlayer.GetDeckCardPurchaseInfo(deckCard).Cost.ToString();
            SetAvailable();
        }


        public void SetChosen(bool isChosen)
        {
            if (isAvailable)
            {
                ifChosenPanel.gameObject.SetActive(isChosen);
            }
        }

        private void SetAvailable()
        {
            var canBuy = Player.LocalPlayer.CanBuyDeckCard(deckCard);
            ifCannotBuyImage.gameObject.SetActive(!canBuy);
            isAvailable = canBuy;
            button.interactable = isAvailable;
            var color = isAvailable ? Color.white : new Color(226 / 255f, 43 / 255f, 18 / 255f, 255 / 255f);
            cost.color = color;
            moneyImage.color = color;
            ifChosenPanel.gameObject.SetActive(false);
            backgroundImage.color = !isAvailable ? Color.gray : new Color(226 / 255f, 43 / 255f, 18 / 255f, 255 / 255f);
            image.color = isAvailable ? Color.white : Color.gray;
            cost.text = Player.LocalPlayer.GetDeckCardPurchaseInfo(deckCard).Cost.ToString();
        }
    }
}