using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class DeckDrawer : MonoBehaviour
    {
        [SerializeField] private CardSlot[] slots;
        [SerializeField] private Button LoadDeckButton;

        public event Action DeckChanged;
        
        private Deck deck;

        public IEnumerable<CardSlot> Slots => slots;

        private void Awake()
        {
            if(LoadDeckButton != null)
                LoadDeckButton.onClick.AddListener(LoadDeck);
            foreach (var slot in slots)
            {
                slot.Init(this);
                slot.CardChanged += SlotCardChanged;
            }
        }

        private void OnEnable()
        {
            LoadDeck();
        }

        private void SlotCardChanged()
        {
            DeckChanged?.Invoke();
        }
        
        public bool SlotCondition(DeckCard deckCard, bool isForBigUnits)
        {
            var a = deckCard.Unit.Size == UnitSize.Large && isForBigUnits || deckCard.Unit.Size == UnitSize.Little && !isForBigUnits;
            var b = !slots.Select(x => x.DeckCard).Contains(deckCard);
            return a && b;
        }

        private void SaveDeck()
        {
            GameRoot.Instance.DecksController.ProcessDeck(GetDeck());
        }

        private void LoadDeck()
        {
            var deck = GameRoot.Instance.DecksController.IdToDeck[0];

            foreach (var slot in slots)
            {
                slot.Clear();
            }
            
            foreach (var card in deck.Cards)
            {
                var slot = slots[0];
                var slotCounter = 0;
                while (true)
                {
                    if (SlotCondition(card, slot.IsForBigUnits) && slot.DeckCard == null)
                    {
                        slot.DrawCard(card);
                        break;
                    }

                    slotCounter++;
                    if (slotCounter == slots.Length)
                        throw new InvalidOperationException("Не удалось загрузить колоду");
                    slot = slots[slotCounter];
                }
            }
            DeckChanged?.Invoke();
        }

        private void OnDisable()
        {
            SaveDeck();
        }

        private Deck GetDeck()
        {
            return new Deck(0, "default", slots.Where(x => x.DeckCard != null).Select(x => x.DeckCard));
        }
    }
}
