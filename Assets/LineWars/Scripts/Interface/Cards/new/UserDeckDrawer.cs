using System;
using System.Collections;
using System.Linq;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Interface
{
    public class UserDeckDrawer: MonoBehaviour
    {
        [SerializeField] private CardDropSlot[] slots;
        [SerializeField] private UserCardsDrawer cardsDrawer;
        private Deck deck => GameRoot.Instance.DecksController.IdToDeck[0];
        private DecksController DecksController => GameRoot.Instance.DecksController;

        private bool dragStarting;
        private bool dragEnding;
        private bool dropped;

        private void OnEnable()
        {
            LoadDeck();
            
            foreach (var slot in slots)
            {
                slot.OnDropCard += SlotOnOnDropCard;
                slot.OnDragEnding += SlotOnDragEnding;
                slot.OnDragStartind += SlotOnDragStarting;
            }
        }

        private void OnDisable()
        {
            foreach (var slot in slots)
            {
                slot.OnDropCard -= SlotOnOnDropCard;
                slot.OnDragEnding -= SlotOnDragEnding;
                slot.Clear();
            }
        }
        
        
        private void SlotOnOnDropCard(CardDropSlot dropSlot, DeckCard oldCard, DeckCard newCard)
        {
            dropped = true;
            
            if (oldCard == null && newCard != null) 
            {
                var otherSlot = slots.FirstOrDefault(x => x.DeckCard == newCard && x != dropSlot);
                if (otherSlot != null) // переложили карту
                    otherSlot.DeckCard = null;
                else if (cardsDrawer != null) // новая карта
                    cardsDrawer.DeckCardToDrawer[newCard].IsActive = false;
            
            }
            else if (oldCard != null && newCard != null)
            {
                if (oldCard == newCard) 
                    return;
                
                var otherSlot = slots.FirstOrDefault(x => x.DeckCard == newCard && x != dropSlot);
                if (otherSlot != null) // своп карты
                {
                    otherSlot.DeckCard = oldCard;
                }
                else if (cardsDrawer != null)
                {
                    cardsDrawer.DeckCardToDrawer[oldCard].IsActive = true;
                    cardsDrawer.DeckCardToDrawer[newCard].IsActive = false;
                }
            }
            
            SaveDeck();
        }
        
        private void SlotOnDragEnding(CardDropSlot slot, DeckCard card)
        {
            if (!dropped)
            {
                slot.DeckCard = null;
                if (cardsDrawer != null) 
                    cardsDrawer.DeckCardToDrawer[card].IsActive = true;
                SaveDeck();
            }
            
            dragStarting = false;
            dragEnding = true;
            dropped = false;
        }
        
        private void SlotOnDragStarting(CardDropSlot slot, DeckCard card)
        {
            dragStarting = true;
            dropped = false;
            dragEnding = false;
        }

        private void LoadDeck()
        {
            foreach (var card in deck.Cards)
            {
                var slot = slots[0];
                var slotCounter = 0;
                while (true)
                {
                    if (slot.DeckCard == null && slot.CanTakeCard(card))
                    {
                        slot.DeckCard = card;
                        if (cardsDrawer != null)
                            cardsDrawer.DeckCardToDrawer[card].IsActive = false;
                        break;
                    }

                    slotCounter++;
                    if (slotCounter == slots.Length)
                        throw new InvalidOperationException("Не удалось загрузить колоду");
                    slot = slots[slotCounter];
                }
            }
        }

        private void SaveDeck()
        { 
            DecksController.ProcessDeck(GetDeck());
        }
        
        private Deck GetDeck()
        {
            return new Deck(0, "default", slots
                .Where(x => x.DeckCard != null)
                .Select(x => x.DeckCard)
            );
        }
    }
}