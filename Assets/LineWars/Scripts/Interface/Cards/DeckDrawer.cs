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

      
        private void Awake()
        {

        }

        public bool SlotCondition(DeckCard deckCard, CardSlot cardSlot)
        {
            return false;
        }
    }
}
