using LineWars.Model;

namespace LineWars.Controllers
{
    public interface IBuyConstrain
    {
        public bool CanSelectNode(Node node);
        public abstract bool CanSelectDeckCard(DeckCard deckCard);
    }
}