using System;
using LineWars.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LineWars.Interface
{
    public class DragableBlessing : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private RectTransform parentTransform;
        [SerializeField] private BlessingInfoDrawer blessingInfoDrawer;
        [SerializeField] private CanvasGroup canvasGroup;
        
        private RectTransform rectTransform;
        private BlessingId blessingId;

        public BlessingId BlessingId => blessingId;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void Init(BlessingId blessingId)
        {
            blessingInfoDrawer.Init(DrawHelper.GetBlessingReDrawInfoByBlessingId(blessingId));
            this.blessingId = blessingId;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / MainCanvas.Instance.Canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(parentTransform);
            transform.localPosition = Vector3.zero;
            canvasGroup.blocksRaycasts = true;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.localPosition = Vector3.zero;
            transform.SetParent(MainCanvas.Instance.Canvas.transform);
            canvasGroup.blocksRaycasts = false;
        }
    }
}
