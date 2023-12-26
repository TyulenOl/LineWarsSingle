using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures;
using JetBrains.Annotations;
using UnityEngine;

namespace LineWars.Model
{
    /// <summary>
    /// Класс ответственный за сохранение целостности данных о деках
    /// </summary>
    public class DecksController : MonoBehaviour
    {
        private IProvider<Deck> deckProvider;
        private Dictionary<int, Deck> allDecks;

        public Deck DeckToGame => IdToDeck[0];
        public IReadOnlyDictionary<int, Deck> IdToDeck => allDecks;
        public IEnumerable<Deck> Decks => allDecks.Values;
        public IEnumerable<int> DeckIds => IdToDeck.Keys;

        public void Initialize([NotNull] IProvider<Deck> provider)
        {
            deckProvider = provider ?? throw new ArgumentNullException(nameof(provider));
            allDecks = provider.LoadAll().ToDictionary(deck => deck.Id, deck => deck);
        }

        /// <summary>
        /// Сохранение деки и внос ее в базу данных
        /// </summary>
        public void ProcessDeck(Deck deck)
        {
            allDecks[deck.Id] = deck;
            deckProvider.Save(deck, deck.Id);
        }
    }
}