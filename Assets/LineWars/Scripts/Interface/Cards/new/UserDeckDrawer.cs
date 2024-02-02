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

        private void OnEnable()
        {
            LoadDeck();
            
            foreach (var slot in slots)
            {
                slot.DeckCardChanged += SlotOnDeckCardChanged;
            }
        }

        private void OnDisable()
        {
            SaveDeck();
            
            foreach (var slot in slots)
            {
                slot.DeckCardChanged -= SlotOnDeckCardChanged;
                slot.Clear();
            }
        }
        
        
        private void SlotOnDeckCardChanged(CardDropSlot dropSlot, DeckCard oldCard, DeckCard newCard)
        {
            // if (oldCard == null && newCard != null) 
            // {
            //     var otherSlot = slots.FirstOrDefault(x => x.DeckCard == newCard && x != dropSlot);
            //     if (otherSlot != null) // переложили карту
            //         otherSlot.DeckCard = null;
            //     else // новая карта
            //         cardsDrawer.DeckCardToDrawer[newCard].IsActive = false;
            //
            // }
            // else if (oldCard != null && newCard == null) 
            // {
            //     if (slots.All(x => x.DeckCard != oldCard)) // удалили карту навсегда
            //         cardsDrawer.DeckCardToDrawer[oldCard].IsActive = true;
            // }
            // else if (oldCard != null && newCard != null)
            // {
            //     var otherSlot = slots.FirstOrDefault(x => x.DeckCard == newCard && x != dropSlot);
            //     if (otherSlot != null) // своп карты
            //     {
            //         otherSlot.DeckCard = oldCard;
            //     }
            //     else 
            //     {
            //         cardsDrawer.DeckCardToDrawer[oldCard].IsActive = true;
            //     }
            // }
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