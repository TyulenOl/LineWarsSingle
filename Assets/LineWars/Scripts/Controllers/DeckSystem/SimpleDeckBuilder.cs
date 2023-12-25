using System;
using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    /// <summary>
    /// Класс описывающий ограничения на сбор деки
    /// PS еще одна точка расширения, на случай изменеия правил колодостроения
    /// </summary>
    public class SimpleDeckBuilder : IDeckBuilder<Deck, DeckCard>
    {
        private int deckId;
        private string deckName = "";
        private readonly List<DeckCard> cards = new();

        public int DeckId => deckId;
        public string DeckName => deckName;

        public SimpleDeckBuilder(int maxLageUnits, int maxLittleUnits)
        {
        }
        
        public void SetId(int id)
        {
            deckId = id;
        }

        public void SetName(string name)
        {
            deckName = name;
        }

        public bool CanAddCard(DeckCard card)
        {
            return !cards.Contains(card);
        }

        public bool AddCard(DeckCard card)
        {
            var canAdd = CanAddCard(card);
            if (!canAdd) return false;

            cards.Add(card);
            return true;
        }

        public bool RemoveCard(DeckCard card)
        {
            var successRemove = cards.Remove(card);
            if (!successRemove) return false;

            return true;
        }

        public IEnumerable<DeckCard> CurrentCards => cards;


        public bool CanBuild()
        {
            return cards.Count > 0;
        }

        public Deck Build()
        {
            return new Deck(deckId, deckName, cards);
        }
    }
}