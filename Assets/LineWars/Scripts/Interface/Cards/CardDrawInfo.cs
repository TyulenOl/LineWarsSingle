using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LineWars
{
    public class CardDrawInfo : MonoBehaviour, IBeginDragHandler, IDragHandler
    {

        [SerializeField] private TMP_Text unitName;
        [SerializeField] private TMP_Text description;
        
        [SerializeField] private TMP_Text hpText;
        [SerializeField] private TMP_Text attackText;
        [SerializeField] private TMP_Text apText;

        [SerializeField] private Image cardImage;
        [SerializeField] private Image ifInactivePanel;
        [SerializeField] private Image borderImage;
        [SerializeField] private CardDragablePart cardDragablePartPrefab;
        

        protected bool isActive;

        public bool IsActive
        {
            get => isActive;

            set
            {
                isActive = value;
            }
        }
        
        [field: SerializeField] public Button InfoButton { get; private set; }

        public DeckCard DeckCard { get; private set; }

        protected UnityAction onInfoButtonClickAction;

        public UnityAction OnInfoButtonClickAction
        {
            get => onInfoButtonClickAction;
            set
            {
                onInfoButtonClickAction = value;
                InfoButton.onClick.AddListener(value);
            }
        }

        public void ReStoreDefaults()
        {
            cardImage.sprite = null;
            unitName.text = "Нет юнита";
            DeckCard = null;
        }
        
        
        public void ReDraw(DeckCard deckCard)
        {
            if(deckCard == null)
            {
                ReStoreDefaults();
                return;
            }
            
            unitName.text = deckCard.Name;
            cardImage.sprite = deckCard.Unit.Sprite;
            
            if (description != null)
                description.text = deckCard.Description;

            if(hpText != null)
                hpText.text = deckCard.Unit.MaxHp.ToString();
            if(attackText != null)
                attackText.text = deckCard.Unit.InitialPower.ToString();
            if(apText != null)
                apText.text = deckCard.Unit.MaxActionPoints.ToString();

            DeckCard = deckCard;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(cardDragablePartPrefab == null || !IsActive)
                return;
            var instance = Instantiate(cardDragablePartPrefab, transform);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.SetParent(MainCanvas.Instance.Canvas.transform);
            instance.ReDraw(DeckCard);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(!IsActive) return;
        }

        public void ReDrawAvailability(bool available)
        {
            IsActive = available;
            borderImage.sprite = available ? DeckCard.CardActiveBagLine : DeckCard.CardInactiveBagLine;   
            if(ifInactivePanel != null)
                ifInactivePanel.gameObject.SetActive(!available);
        }
    }
}
