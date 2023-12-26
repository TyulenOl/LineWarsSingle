namespace LineWars.Model
{
    public class BuyPresetOnNodeCommand : ICommand
    {
        public BasePlayer Player { get; private set; }
        public Node Node {  get; private set; } 
        public DeckCard DeckCard { get; private set; }

        public BuyPresetOnNodeCommand(BasePlayer basePlayer, Node node, DeckCard deckCard)
        {
            Player = basePlayer;
            Node = node;
            DeckCard = deckCard;
        }

        public bool CanExecute()
        {
            return Player.CanBuyDeckCard(Node, DeckCard);
        }

        public void Execute()
        {
            Player.BuyDeckCard(Node, DeckCard);
        }

        public string GetLog()
        {
            return $"Player {Player} buy preset {DeckCard.Name} in node {Node}";
        }
    }
}
