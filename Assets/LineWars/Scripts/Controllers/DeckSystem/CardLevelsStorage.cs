namespace LineWars.Model
{
    public class CardLevelsStorage
    {
        private IStorage<int, DeckCard> cardStorage;
        private IReadOnlyUserInfo userInfo;

        public CardLevelsStorage(IStorage<int, DeckCard> cardStorage, IReadOnlyUserInfo userInfo) 
        { 
            this.cardStorage = cardStorage;
            this.userInfo = userInfo;

        }

        public IReadOnlyCardLevelInfo GetCardLevelInfo(int cardId)
        {
            var currentLevel = userInfo.CardLevels[cardId];
            var card = cardStorage.IdToValue[cardId];
            return card.GetLevel(currentLevel);
        }

        public IReadOnlyCardLevelInfo GetCardLevelInfo(DeckCard card)
        {
            var cardId = cardStorage.ValueToId[card];
            return card.GetLevel(cardId);
        }
    }
}
