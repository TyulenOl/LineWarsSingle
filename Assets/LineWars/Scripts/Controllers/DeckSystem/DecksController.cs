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
    public class DecksController: MonoBehaviour
    {
        [SerializeField] private DeckBuilderFactory deckBuilderFactory;

        private IProvider<Deck> provider;
        private List<Deck> allDecks;
        private ExclusionarySequence sequence;
        
        public void Initialize([NotNull] IProvider<Deck> provider)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
            allDecks = provider.LoadAll().ToList();
            sequence = new ExclusionarySequence(0, allDecks.Select(x => x.Id));
        }
        
        public IDeckBuilder<Deck, DeckCard> StartBuildNewDeck()
        {
            var builder = deckBuilderFactory.CreateNew();
            builder.SetId(sequence.Peek());
            return builder;
        }

        public Deck FinishBuildDeck(IDeckBuilder<Deck, DeckCard> deckBuilder)
        {
            var deck = deckBuilder.Build();
            provider.Save(deck, deck.Id);
            sequence.Pop();
            allDecks.Add(deck);
            return deck;
        }
    }
}