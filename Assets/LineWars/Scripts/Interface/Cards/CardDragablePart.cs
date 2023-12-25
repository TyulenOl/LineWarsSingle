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
            rectTransform.anchoredPosition += eventData.delta / MainCanvas.Instance.Canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Destroy(gameObject);
            canvasGroup.blocksRaycasts = true;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            var instance = Instantiate(this, rectTransformToGenerateCard);
            instance.transform.localPosition = Vector3.zero;
            instance.ReDraw(DeckCard);
            transform.SetParent(MainCanvas.Instance.Canvas.transform);
            canvasGroup.blocksRaycasts = false;
        }
    }
}
