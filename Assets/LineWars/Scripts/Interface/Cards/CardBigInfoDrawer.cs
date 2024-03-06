using System.Collections;
using System.Collections.Generic;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LineWars
{
    public class CardBigInfoDrawer : MonoBehaviour
    {
        [SerializeField] private TMP_Text unitName;
        [SerializeField] private TMP_Text description;

        [SerializeField] private Image cardImage;

        [SerializeField] private ActionsPanelDrawer actionsPanelDrawer;
        [SerializeField] private CardUpgradeLogic cardUpgradeLogic;
        private DeckCard deckCard;

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
                cardUpgradeLogic.DeckCard = deckCard;
                
                Redraw(value);
            }
        }

        private void DeckCardOnLevelChanged(DeckCard card, int level)
        {
            Redraw(card);
        }

        private void Redraw(DeckCard deckCard)
        {
            if (deckCard == null)
                return;
            
            unitName.text = deckCard.Name;
            cardImage.sprite = deckCard.Unit.Sprite;
            
            if (description != null)
                description.text = deckCard.Description;
            
            actionsPanelDrawer.ReDrawActions(deckCard);
        }
    }
}
