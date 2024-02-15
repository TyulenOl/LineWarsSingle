using System;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class CardDraggableSet: MonoBehaviour
    {
        [SerializeField] private BaseCardDrawer staticDrawer;
        [SerializeField] private DelayDraggableCard draggableCard;
        
        
        private DeckCard deckCard;
        [SerializeField, ReadOnlyInspector]private bool isActive = true;

        public DeckCard DeckCard
        {
            get => deckCard;
            set
            {
                staticDrawer.DeckCard = value;
                draggableCard.DeckCard = value;
                deckCard = value;
            }
        }

        public bool IsActive
        {
            get => isActive;
            set
            {
                isActive = value;
                draggableCard.IsActive = value;
                staticDrawer.IsActive = value;

                staticDrawer.gameObject.SetActive(!value);
            }
        }

        public ScrollRect ScrollRect
        {
            get => draggableCard.ScrollRect;
            set => draggableCard.ScrollRect = value;
        }

        public UnityEvent OnClick => draggableCard.OnClick;

        private void Start()
        {
            draggableCard.StartDragging += DraggableCardOnStartDragging;
            draggableCard.EndDragging += DraggableCardOnEndDragging;
        }
        
        private void OnDestroy()
        {
            if (draggableCard != null)
            {
                draggableCard.StartDragging -= DraggableCardOnStartDragging;
                draggableCard.EndDragging -= DraggableCardOnEndDragging;
            }
        }

        private void DraggableCardOnStartDragging(DraggableCard card)
        {
            staticDrawer.gameObject.SetActive(true);
            staticDrawer.IsActive = false;
        }
        private void DraggableCardOnEndDragging(DraggableCard card)
        {
            staticDrawer.gameObject.SetActive(false);
        }

        public void RedrawLevelInfo()
        {
            staticDrawer.RedrawUpdateInfo();
            draggableCard.RedrawLevelInfo();
        }
    }
}