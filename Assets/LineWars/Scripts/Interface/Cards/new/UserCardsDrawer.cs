using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class UserCardsDrawer: MonoBehaviour
    {
        [SerializeField] private CardBigInfoDrawer bigCardInfo;
        [SerializeField] private CardDraggableSet cardPrefab;
        [SerializeField] private LayoutGroup cardsLayoutGroup;

        private readonly Dictionary<DeckCard, CardDraggableSet> deckCardToDrawer = new ();

        public IReadOnlyDictionary<DeckCard, CardDraggableSet> DeckCardToDrawer => deckCardToDrawer;

        private UserInfoController UserInfoController => GameRoot.Instance.UserController;

        private void OnEnable()
        {
            ReDrawAllCards();
        }

        private void OnDisable()
        {
            DestroyAllCards();
        }
        
        private void ReDrawAllCards()
        {
            foreach (var card in UserInfoController.OpenedCards
                         .OrderBy(x => x.Unit.Size)
                         .ThenBy(x=> x.Unit.Type))
            {
                var instance = Instantiate(cardPrefab, cardsLayoutGroup.transform);

                instance.DeckCard = card;
                instance.OnClick.AddListener(() =>
                {
                    bigCardInfo.gameObject.SetActive(true);
                    bigCardInfo.DeckCard = card;
                });

                deckCardToDrawer[card] = instance;
                
                card.LevelChanged += CardOnLevelChanged;
            }
        }
        private void DestroyAllCards()
        {
            foreach (var drawer in deckCardToDrawer.Values)
            {
                drawer.DeckCard.LevelChanged -= CardOnLevelChanged;
                Destroy(drawer.gameObject);
            }
            deckCardToDrawer.Clear();
        }
        
        private void CardOnLevelChanged(DeckCard deckCard, int level)
        {
            foreach (var value in deckCardToDrawer.Values)
                value.RedrawLevelInfo();
        }
    }
}