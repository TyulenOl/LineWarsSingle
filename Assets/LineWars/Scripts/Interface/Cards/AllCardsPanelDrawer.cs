using System.Linq;
using LineWars.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars
{
    public class AllCardsPanelDrawer : MonoBehaviour
    {
        [SerializeField] private LayoutGroup cardsLayoutGroup;
        [SerializeField] private RectTransform cardDrawInfoPrefab;
        [SerializeField] private CardBigInfoDrawer bigCardInfo;
        [SerializeField] private DeckDrawer deckDrawer;
        

        private void Start()
        {
            ReDrawAllCards();
            deckDrawer.DeckChanged += ReDrawAllCards;
        }

        private void ReDrawAllCards()
        {
            Debug.Log("ReDrawAllCards");
            foreach (var drawInfo in cardsLayoutGroup.GetComponentsInChildren<CardDrawInfo>())
            {
                Destroy(drawInfo.transform.parent.gameObject);
            }
            
            var cards = GameRoot.Instance.UserController.OpenedCards;
            foreach (var card in cards)
            {
                var instance = Instantiate(cardDrawInfoPrefab, cardsLayoutGroup.transform);
                var cardInstance = instance.GetComponentInChildren<CardDrawInfo>();
                cardInstance.ReDraw(card);
                cardInstance.OnInfoButtonClickAction = () =>
                {
                    bigCardInfo.gameObject.SetActive(true);
                    bigCardInfo.DeckCard = card;
                };
                cardInstance.ReDrawAvailability(!deckDrawer.Slots.Select(x=> x.DeckCard).Contains(cardInstance.DeckCard));
            }
        }
    }
}
