using LineWars.Controllers;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Nox7atra.UIFigmaGradients;

namespace LineWars.Interface
{
    public class BaseCardDrawer: MonoBehaviour
    {
        [SerializeField] private TMP_Text unitName;
        
        [SerializeField] private TMP_Text hpText;
        [SerializeField] private TMP_Text attackText;
        [SerializeField] private TMP_Text apText;
        [SerializeField] private RectTransform upgradeDrawer;
        
        [SerializeField] private Image cardImage;
        [SerializeField] private Image ifInactivePanel;
        [SerializeField] private UIFigmaGradientLinearDrawer border;
        
        private CardUpgrader CardUpgrader => GameRoot.Instance.CardUpgrader;

        private DeckCard deckCard;
        [SerializeField, ReadOnlyInspector] private bool isActive = true;

        public bool IsActive
        {
            get => isActive;
            set
            {
                // if (borderImage != null)
                //     borderImage.sprite = value ? DeckCard.CardActiveBagLine : DeckCard.CardInactiveBagLine;   
                if(ifInactivePanel != null)
                    ifInactivePanel.gameObject.SetActive(!value);
                isActive = value;

                if (!value)
                {
                    upgradeDrawer.gameObject.SetActive(false);
                }
                else
                {
                    upgradeDrawer.gameObject.SetActive(deckCard != null && CardUpgrader.CanUpgrade(deckCard));
                }
            }
        }

        public DeckCard DeckCard
        {
            get => deckCard;
            set
            {
                if (deckCard != null)
                    deckCard.LevelChanged -= DeckCardOnLevelChanged;
                if (value != null)
                    value.LevelChanged += DeckCardOnLevelChanged;
                
                deckCard = value;
                Redraw(deckCard);
            }
        }

        private void DeckCardOnLevelChanged(DeckCard deckCard, int level)
        {
            Redraw(deckCard);
        }

        private void Redraw(DeckCard deckCard)
        {
            if(deckCard == null)
            {
                ReStoreDefaults();
                return;
            }
            
            if (unitName != null)
                unitName.text = deckCard.Name;
            if (cardImage != null)
                cardImage.sprite = deckCard.Image;
            if(hpText != null)
                hpText.text = deckCard.Unit.MaxHp.ToString();
            if(attackText != null)
                attackText.text = deckCard.Unit.InitialPower.ToString();
            if(apText != null)
                apText.text = deckCard.Unit.MaxActionPoints.ToString();

            if (border != null)
            {
                switch (DeckCard.Rarity)
                {
                    case Rarity.Common:
                        border.SetGradient(DrawHelper.TypeToGradient[GradientType.Common]);
                        break;
                    default: 
                        border.SetGradient(DrawHelper.TypeToGradient[GradientType.Epic]);
                        break;
                }
               
            }
            // if (borderImage != null)
            //     borderImage.sprite = DeckCard.CardActiveBagLine;
                
            if (upgradeDrawer != null)
            {
                var active = IsActive && CardUpgrader.CanUpgrade(deckCard);
                upgradeDrawer.gameObject.SetActive(active);
            }
        }
        
        private void ReStoreDefaults()
        {
            cardImage.sprite = null;
            unitName.text = "Нет юнита";
        }
    }
}