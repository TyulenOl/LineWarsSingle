using System;
using System.Collections;
using LineWars.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class CardDropSlot: MonoBehaviour,
        IDropHandler,
        IPointerEnterHandler, 
        IPointerExitHandler
    {
        [SerializeField] private DraggableCard draggableCard;
        [SerializeField] private RectTransform ifUnavailablePanel;
        [SerializeField] private RectTransform ifAvailablePanel;
        [SerializeField] private RectTransform emptySlotTransform;

        [SerializeField] private UnitSize unitSize;
        private DeckCard deckCard;
        public DeckCard DeckCard
        {
            get => deckCard;
            set
            {
                var prevValue = deckCard;
                deckCard = value;
                draggableCard.DeckCard = value;
                
                Redraw(value);

                if (prevValue != deckCard)
                {
                    DeckCardChanged?.Invoke(this, prevValue, deckCard);
                }
            }
        }
        
        public event Action<CardDropSlot ,DeckCard, DeckCard> DeckCardChanged;

        private void Start()
        {
            draggableCard.StartDragging += DraggableCardOnStartDragging;
            draggableCard.EndDragging += DraggableCardOnEndDragging;
        }

        private void OnDestroy()
        {
            draggableCard.StartDragging -= DraggableCardOnStartDragging;
            draggableCard.EndDragging -= DraggableCardOnEndDragging;
        }

        private void DraggableCardOnStartDragging(DraggableCard obj)
        {
            emptySlotTransform.gameObject.SetActive(true);
        }
        
        private void DraggableCardOnEndDragging(DraggableCard obj)
        {
            
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData == null
                || eventData.pointerDrag == null
                || !eventData.pointerDrag.TryGetComponent(out DraggableCard draggableCard))
                return;
            if (draggableCard.DeckCard == null)
                Debug.LogError("Draggable card missing card!");
            
            if (!CanTakeCard(draggableCard.DeckCard))
                return;
            
            DeckCard = draggableCard.DeckCard;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(eventData.pointerDrag == null)
                return;
            var dragableItem = eventData.pointerDrag.GetComponent<DraggableCard>();
            if(dragableItem == null)
                return;
            ShowAvailability(CanTakeCard(dragableItem.DeckCard));
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            ifAvailablePanel.gameObject.SetActive(false);
            ifUnavailablePanel.gameObject.SetActive(false);
        }
        
        private void Redraw(DeckCard deck)
        {
            if (deck != null)
            {
                ifAvailablePanel.gameObject.SetActive(false);
                ifUnavailablePanel.gameObject.SetActive(false);
                emptySlotTransform.gameObject.SetActive(false);
                draggableCard.gameObject.SetActive(true);
            }
            else
            {
                emptySlotTransform.gameObject.SetActive(true);
                draggableCard.EndDrag();
                draggableCard.gameObject.SetActive(false);
            }
        }
        
        
        private void ShowAvailability(bool isAvailable)
        {
            ifAvailablePanel.gameObject.SetActive(isAvailable);
            ifUnavailablePanel.gameObject.SetActive(!isAvailable);
        }
        
        public bool CanTakeCard(DeckCard deckCard)
        {
            return deckCard.Unit.Size == unitSize && DeckCard != deckCard;
        }
        
        public void Clear()
        {
            DeckCard = null;
        }

        private IEnumerator ChangeDeckCardCoroutine(DeckCard oldValue, DeckCard newValue)
        {
            yield return new WaitForFixedUpdate();
            DeckCardChanged?.Invoke(this, oldValue, newValue);
        }
    }
}