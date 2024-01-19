using LineWars.Model;
using UnityEngine;

namespace LineWars.Education
{
    public class BuyPlayerAction: PlayerAction
    {
        [Space]
        [SerializeField] private Node buyNode;
        [SerializeField] private DeckCard deckCard;

        public override bool CanSelectNode(Node node)
        {
            return buyNode.Equals(node);
        }

        public override bool CanSelectDeckCard(DeckCard deckCard)
        {
            return deckCard != null || this.deckCard == deckCard;
        }
    }
}