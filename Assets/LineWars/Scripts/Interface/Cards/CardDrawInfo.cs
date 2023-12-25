using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LineWars
{
    public class CardDrawInfo : MonoBehaviour, IBeginDragHandler, IDragHandler
    {

        [SerializeField] private TMP_Text unitName;
        [SerializeField] private TMP_Text description;
        
        [SerializeField] private TMP_Text hpText;
        [SerializeField] private TMP_Text armorText;
        [SerializeField] private TMP_Text attackText;
        [SerializeField] private TMP_Text apText;

        [SerializeField] private Image cardImage;

        [SerializeField] private CardDragablePart cardDragablePartPrefab;
        
        [field: SerializeField] public Button InfoButton { get; private set; }

        public DeckCard DeckCard { get; private set; }

        public void ReDraw(DeckCard deckCard)
        {
            unitName.text = deckCard.Name;
            cardImage.sprite = deckCard.Unit.Sprite;
            
            if (description != null)
                description.text = deckCard.Description;

            if(hpText != null)
                hpText.text = deckCard.Unit.MaxHp.ToString();
            if(armorText != null)
                armorText.text = deckCard.Unit.MaxArmor.ToString();
            if(attackText != null)
                attackText.text = 0.ToString() ;//deckCard.Unit.GetMaxDamage().ToString();
            if(apText != null)
                apText.text = deckCard.Unit.MaxActionPoints.ToString();

            DeckCard = deckCard;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log(1);
            var instance = Instantiate(cardDragablePartPrefab, transform);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.SetParent(MainCanvas.Instance.Canvas.transform);
            instance.ReDraw(DeckCard);
        }

        public void OnDrag(PointerEventData eventData)
        {
            
        }
    }
}
