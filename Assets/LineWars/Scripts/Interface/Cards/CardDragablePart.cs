using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LineWars
{
    public class CardDragablePart : CardDrawInfo, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        
        [SerializeField] private RectTransform rectTransformToGenerateCard;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(!IsActive) return;
            rectTransform.anchoredPosition += eventData.delta / MainCanvas.Instance.Canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(!IsActive) return;
            Destroy(gameObject);
            canvasGroup.blocksRaycasts = true;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log(IsActive);
            if(!IsActive)
                return;
            var instance = Instantiate(this, rectTransformToGenerateCard);
            instance.transform.localPosition = Vector3.zero;
            instance.ReDraw(DeckCard);
            instance.rectTransformToGenerateCard = rectTransformToGenerateCard;
            instance.canvasGroup.blocksRaycasts = true;
            instance.isActive = true;
            instance.InfoButton.onClick.AddListener(onInfoButtonClickAction);
            transform.SetParent(MainCanvas.Instance.Canvas.transform);
            canvasGroup.blocksRaycasts = false;
        }
    }
}
