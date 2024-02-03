using System;
using LineWars.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LineWars.Interface
{
    public class DraggableCard: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private BaseCardDrawer baseCardDrawer;
        [SerializeField] private RectTransform parentTransform;
        [SerializeField] private CanvasGroup canvasGroup;

        public CanvasGroup CanvasGroup => canvasGroup;

        private DeckCard deckCard;

        private RectTransform rectTransform;
        private bool isActive = true;

        private bool isDragging;
        
        public bool IsActive
        {
            get => isActive;
            set
            {
                isActive = value;
                baseCardDrawer.IsActive = value;
            }
        }

        public DeckCard DeckCard
        {
            get => deckCard;
            set
            {
                baseCardDrawer.DeckCard = value;
                deckCard = value;
            }
        }

        public event Action<DraggableCard> StartDragging;
        public event Action<DraggableCard> EndDragging;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
            
            transform.localPosition = Vector3.zero;
            transform.SetParent(MainCanvas.Instance.Canvas.transform);
            canvasGroup.blocksRaycasts = false;
            
            StartDragging?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(!IsActive || !isDragging) return;
            
            rectTransform.anchoredPosition += eventData.delta / MainCanvas.Instance.Canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EndDrag();
        }

        public void EndDrag()
        {
            if (!isDragging)
                return;
            
            isDragging = false;
            transform.SetParent(parentTransform);
            transform.localPosition = Vector3.zero;
            canvasGroup.blocksRaycasts = true;
            
            EndDragging?.Invoke(this);
        }
    }
}