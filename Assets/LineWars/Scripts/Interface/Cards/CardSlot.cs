using LineWars.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LineWars
{
    public class CardSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CardDrawInfo cardDrawInfo;
        [SerializeField] private RectTransform ifUnavailablePanel;
        [SerializeField] private RectTransform ifAvailablePanel;

        [SerializeField] private bool isForBigUnits;

        private DeckDrawer deckDrawer;

        public void Init(DeckDrawer deckDrawer)
        {
            this.deckDrawer = deckDrawer;
        }
        
        
        public void OnDrop(PointerEventData eventData)
        {
            
            var dragableItem = eventData.pointerDrag.GetComponent<CardDragablePart>();
            if(dragableItem == null) return;
            if(!CardCondition(dragableItem.DeckCard))
                return;
            cardDrawInfo.ReDraw(dragableItem.DeckCard);
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
            return deckCard.Unit.Size == UnitSize.Large && isForBigUnits || deckCard.Unit.Size == UnitSize.Little && !isForBigUnits;
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