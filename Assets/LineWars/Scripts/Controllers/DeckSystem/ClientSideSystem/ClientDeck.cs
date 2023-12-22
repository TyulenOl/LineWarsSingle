using LineWars.Controllers;

namespace LineWars.Model
{
    public class ClientDeck: DownloaderConvertDecorator<Deck, DeckInfo>
    {
        public ClientDeck(
            IDownloader<DeckInfo> innerLoader,
            IConverter<DeckInfo, Deck> converter) : base(innerLoader, converter)
        {
        }
    }
}