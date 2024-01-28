using LineWars.Model;

namespace LineWars.Controllers
{
    public class SingleGameDeckGetter: DeckGetter
    {
        public override bool CanGet()
        {
            return true;
        }

        public override Deck Get()
        {
            if (GameRoot.Instance != null)
                return GameRoot.Instance.DecksController.DeckToGame;
            return new Deck(0)
            {
                Name = "Deck"
            };
        }
    }
}