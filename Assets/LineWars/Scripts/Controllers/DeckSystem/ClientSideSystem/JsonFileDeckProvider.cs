using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    
    [CreateAssetMenu(menuName = "Providers/JsonFileDeckProvider", order = 52)]
    public class JsonFileDeckProvider : DeckProvider
    {
        private ISaver<Deck> saver;
        private IDownloader<Deck> downloader;
        private IAllDownloader<Deck> allDownloader;

        public JsonFileDeckProvider Initialize(
            IReadOnlyDictionary<int, DeckCard> idToCard,
            IReadOnlyDictionary<DeckCard, int> cardToId)
        {
            saver = new SaverConvertDecorator<Deck, DeckInfo>(
                new JsonFileSaver<DeckInfo>(),
                new DeckToInfoConverter(cardToId)
            );

            downloader = new DownloaderConvertDecorator<Deck, DeckInfo>(
                new JsonFileLoader<DeckInfo>(),
                new DeckInfoToDeckConverter(idToCard)
            );

            allDownloader = new AllDownloaderConvertDecorator<Deck, DeckInfo>(
                new JsonFileAllDownloader<DeckInfo>(),
                new DeckInfoToDeckConverter(idToCard));

            return this;
        }

        public override void Save(Deck value, int id) => saver.Save(value, id);
        public override Deck Load(int id) => downloader.Load(id);
        public override IEnumerable<Deck> LoadAll() => allDownloader.LoadAll();
    }
}