﻿using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using DataStructures;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    /// <summary>
    /// Класс ответственный за сохранение целостности данных о деках
    /// </summary>
    public class DecksController : MonoBehaviour,
        IDeckIndexer
    {
        [SerializeField] List<DeckCard> defaultDeck;
        
        private UserInfoController userInfoController;
        private IProvider<Deck> deckProvider;
        private Dictionary<int, Deck> allDecks;

        public Deck DeckToGame => IdToDeck[0];
        public IDeckIndexer IdToDeck => this;
        public IEnumerable<Deck> Decks => allDecks.Values;
        public IEnumerable<int> DeckIds => allDecks.Keys;

        public Deck this[int id]
        {
            get
            {
                allDecks.TryAdd(id, new Deck(id));
                return allDecks[id];
            }
        }

        public void Initialize([NotNull] IProvider<Deck> provider, UserInfoController userInfoController)
        {
            this.userInfoController = userInfoController;
            deckProvider = provider ?? throw new ArgumentNullException(nameof(provider));
            allDecks = provider.LoadAll()
                .Select(AssignDeck)
                .ToDictionary(deck => deck.Id, deck => deck);
            TryAddDefaultDeck();
        }

        /// <summary>
        /// Сохранение деки и внос ее в базу данных
        /// </summary>
        public void ProcessDeck([NotNull] Deck deck)
        {
            if (deck == null) throw new ArgumentNullException(nameof(deck));

            var isEquals = allDecks.ContainsKey(deck.Id) && deck.Equals(allDecks[deck.Id]);
            allDecks[deck.Id] = deck;
            
            if (!isEquals)
                SaveDeck(deck);
        }

        private Deck AssignDeck(Deck deck)
        {
            return new Deck(deck.Id, deck.Name, deck.Cards
                .Distinct()
                .Where(x => userInfoController.CardIsOpen(x))
            );
        }

        private void TryAddDefaultDeck()
        {
            if (!allDecks.ContainsKey(0))
            {
                //Debug.Log("TryAddDefaultDeck");
                var deck = new Deck(0);
                foreach (var card in defaultDeck)
                    deck.AddCard(card);
                deck = AssignDeck(deck);
                SaveDeck(deck);
                allDecks[0] = deck;
            }
        }

        private void SaveDeck(Deck deck)
        {
            deckProvider.Save(deck, deck.Id);
        }
    }

    public interface IDeckIndexer
    {
        public Deck this[int id]
        {
            get;
        }
    }
}