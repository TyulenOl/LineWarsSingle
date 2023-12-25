using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class DeckDrawer : MonoBehaviour
    {
        [SerializeField] private CardSlot[] slots;

        private SimpleDeckBuilder simpleDeckBuilder;

        private void Awake()
        {
            simpleDeckBuilder = GameRoot.Instance.DecksController.StartEditDeck(GameRoot.Instance.DecksController.DefaultDeck) as SimpleDeckBuilder;
        }

        public bool SlotCondition(DeckCard deckCard, CardSlot cardSlot)
        {
            return simpleDeckBuilder.CanAddCard(deckCard);
        }
    }
}
