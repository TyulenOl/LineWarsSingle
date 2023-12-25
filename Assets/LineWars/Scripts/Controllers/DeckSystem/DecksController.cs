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
        [SerializeField] private DeckBuilderFactory deckBuilderFactory;
        [SerializeField] private DefaultDeck defaultDeck;
        
        private IProvider<Deck> deckProvider;
        private Dictionary<int, Deck> allDecks;
        private ExclusionarySequence sequence;

        private IReadOnlyDictionary<int, Deck> IdToDeck => allDecks;
        private IEnumerable<Deck> Decks => allDecks.Values;
        private IEnumerable<int> DeckIds => IdToDeck.Keys;

        public void Initialize([NotNull] IProvider<Deck> provider)
        {
            deckProvider = provider ?? throw new ArgumentNullException(nameof(provider));
            allDecks = provider.LoadAll().ToDictionary(deck => deck.Id, deck => deck);
            sequence = new ExclusionarySequence(0, allDecks.Select(x => x.Key));
            if (allDecks.Count == 0)
            {
                allDecks.Add(0, new Deck(0, defaultDeck.Name, defaultDeck.Cards));
                sequence.Pop();
            }
        }
        
        public IDeckBuilder<Deck, DeckCard> StartBuildNewDeck()
        {
            var builder = deckBuilderFactory.CreateNew();
            builder.SetId(sequence.Peek());
            return builder;
        }

        public IDeckBuilder<Deck, DeckCard> StartEditDeck(Deck deck)
        {
            return deckBuilderFactory.CreateFromOtherDeck(deck);
        }

        public Deck FinishBuildDeck(IDeckBuilder<Deck, DeckCard> deckBuilder)
        {
            var deck = deckBuilder.Build();
            deckProvider.Save(deck, deck.Id);
            sequence.Pop();
            allDecks.Add(deck.Id, deck);
            return deck;
        }
    }
}