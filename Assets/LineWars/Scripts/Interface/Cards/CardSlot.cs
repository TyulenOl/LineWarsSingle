using System;
using LineWars.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LineWars
{
    [Obsolete("Use CardDropSlot")]
    public class CardSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CardDragablePart cardDrawInfo;
        [SerializeField] private RectTransform ifUnavailablePanel;
        [SerializeField] private RectTransform ifAvailablePanel;
        [SerializeField] private RectTransform emptySlotTransform;
        [SerializeField] private bool isForBigUnits;

        private DeckDrawer deckDrawer;
        
        public event Action CardChanged;
        
        public DeckCard DeckCard => cardDrawInfo.DeckCard;

        public bool IsForBigUnits => isForBigUnits;

        public void Init(DeckDrawer deckDrawer)
        {
            this.deckDrawer = deckDrawer;
            cardDrawInfo.StartDragging += CardStartDragging;
        }


        public void Clear()
        {
            cardDrawInfo.gameObject.SetActive(false);
            emptySlotTransform.gameObject.SetActive(true);
            cardDrawInfo.ReStoreDefaults();
            CardChanged?.Invoke();
        }
        

        public void DrawCard(DeckCard deckCard)
        {
            if(deckCard == null)
                return;
            if(!CardCondition(deckCard))
                return;
            cardDrawInfo.gameObject.SetActive(true);
            emptySlotTransform.gameObject.SetActive(false);
            cardDrawInfo.ReDraw(deckCard);
            cardDrawInfo.ReDrawAvailability(true);
            ifAvailablePanel.gameObject.SetActive(false);
            ifUnavailablePanel.gameObject.SetActive(false);
        }

        private void CardStartDragging(CardDragablePart cardDragablePart)
        {
            cardDrawInfo.StartDragging -= CardStartDragging;
            cardDrawInfo = cardDragablePart;
            cardDrawInfo.StartDragging += CardStartDragging;
            Clear();
        }


        public void OnDrop(PointerEventData eventData)
        {
            var dragableItem = eventData.pointerDrag.GetComponent<CardDragablePart>();
            if(dragableItem == null) return;
            DrawCard(dragableItem.DeckCard);
            CardChanged?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(eventData.pointerDrag == null)
                return;
            var dragableItem = eventData.pointerDrag.GetComponent<CardDragablePart>();
            if(dragableItem == null)
                return;
            ShowAvailability(CardCondition(dragableItem.DeckCard));
        }

        private bool CardCondition(DeckCard deckCard)
        {
            return deckDrawer.SlotCondition(deckCard, isForBigUnits);
        }
        
        private void ShowAvailability(bool isAvailable)
        {
            ifAvailablePanel.gameObject.SetActive(isAvailable);
            ifUnavailablePanel.gameObject.SetActive(!isAvailable);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ifAvailablePanel.gameObject.SetActive(false);
            ifUnavailablePanel.gameObject.SetActive(false);
        }
    }
}