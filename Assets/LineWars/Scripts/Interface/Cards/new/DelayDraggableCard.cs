using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class DelayDraggableCard: DraggableCard,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerExitHandler
    {
        [SerializeField] private float delay = 0.5f;
        [SerializeField] private GameObject ifCanStartDraggingPanel;
        public ScrollRect ScrollRect { get; set; }
        private bool delayDragging;
        private bool dragStarting;
        private Vector2 dragDelta;


        public UnityEvent OnClick;
        public bool DragStarting => dragStarting;

        public void OnPointerDown(PointerEventData eventData)
        {
            dragDelta = Vector2.zero;
            
            if (!IsActive)
            {
                return;
            }

            StartCoroutine(StartTimer());
        }

        public void OnPointerExit(PointerEventData eventData)
        { 
            StopAllCoroutines();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!delayDragging && dragDelta.magnitude == 0)
                OnClick.Invoke();
            if (ifCanStartDraggingPanel)
                ifCanStartDraggingPanel.gameObject.SetActive(false);
            StopAllCoroutines();
        }

        private IEnumerator StartTimer()
        {
            yield return new WaitForSeconds(delay);
            delayDragging = true;
            if (ifCanStartDraggingPanel)
                ifCanStartDraggingPanel.gameObject.SetActive(true);
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            ExecuteEvents.Execute(ScrollRect.gameObject, eventData, ExecuteEvents.beginDragHandler);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            dragDelta = eventData.delta;
            if (delayDragging)
            {
                if (!dragStarting)
                {
                    dragStarting = true;
                    StartDrag();
                }
                Drag(eventData);
            } 
            else
            {
                ExecuteEvents.Execute(ScrollRect.gameObject, eventData, ExecuteEvents.dragHandler);
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            ExecuteEvents.Execute(ScrollRect.gameObject, eventData, ExecuteEvents.endDragHandler);
            if (delayDragging)
            {
                delayDragging = false;
                dragStarting = false;
                if (ifCanStartDraggingPanel)
                    ifCanStartDraggingPanel.gameObject.SetActive(false);
                EndDrag();
            }
        }
    }
}