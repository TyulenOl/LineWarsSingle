using System;
using DataStructures;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    public class GameRoot: DontDestroyOnLoadSingleton<GameRoot>
    {
        [SerializeField] private ScriptableDeckCardsStorage cardsDatabase;
        [SerializeField] private DeckProvider deckProvider;
        [SerializeField] private DecksController decksController;
        
        public DecksController DecksController => decksController;
        public ScriptableDeckCardsStorage CardsDatabase => cardsDatabase;
        

        protected override void OnAwake()
        {
            ValidateFields();
            //так как нет конструкторов пришлось так
            switch (deckProvider)
            {
                case JsonFileDeckProvider jsonFileDeckProvider:
                    jsonFileDeckProvider.Initialize(cardsDatabase.IdToCard, cardsDatabase.CardToId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            DecksController.Initialize(deckProvider);
        }

        private void ValidateFields()
        {
            if (cardsDatabase == null)
                Debug.LogError($"{nameof(cardsDatabase)} is null on {name}!", gameObject);
            if (deckProvider == null)
                Debug.LogError($"{nameof(deckProvider)} is null on {name}!", gameObject);
            if (decksController == null)
                Debug.LogError($"{nameof(decksController)} is null on {name}!", gameObject);
        }
    }
}