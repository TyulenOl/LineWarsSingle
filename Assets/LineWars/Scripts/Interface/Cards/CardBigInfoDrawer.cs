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
        
        [SerializeField] private TMP_Text hpText;
        [SerializeField] private TMP_Text armorText;
        [SerializeField] private TMP_Text attackText;
        [SerializeField] private TMP_Text apText;

        [SerializeField] private Image cardImage;

        [SerializeField] private ActionsPanelDrawer actionsPanelDrawer;
        
        public void ReDraw(DeckCard deckCard)
        {
            unitName.text = deckCard.Name;
            cardImage.sprite = deckCard.Unit.Sprite;
            
            if (description != null)
                description.text = deckCard.Description;

            if(hpText != null)
                hpText.text = deckCard.Unit.MaxHp.ToString();
            if (attackText != null)
                attackText.text = deckCard.Unit.InitialPower.ToString(); ;//deckCard.Unit.GetMaxDamage().ToString();
            if(apText != null)
                apText.text = deckCard.Unit.MaxActionPoints.ToString();
            
            actionsPanelDrawer.ReDrawActions(deckCard);
        }
    }
}
