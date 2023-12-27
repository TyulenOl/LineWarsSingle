namespace LineWars.Model
{
    public class BuyDeckCardOnNodeCommand : ICommand
    {
        public BasePlayer Player { get; }
        public Node Node {  get; } 
        public DeckCard DeckCard { get; }

        public BuyDeckCardOnNodeCommand(BasePlayer basePlayer, Node node, DeckCard deckCard)
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
