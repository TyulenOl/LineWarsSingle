using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace LineWars.Model
{
    public class ClientAllDecksDownloader : IAllDownloader<Deck>
    {
        private IConverter<DeckInfo, Deck> infoConverter;

        public ClientAllDecksDownloader(IConverter<DeckInfo, Deck> infoConverter)
        {
            this.infoConverter = infoConverter;
        }

        public IEnumerable<Deck> LoadAll()
        {
            var directoryPath = ClientDeckInfoSystemUtilities.DeckDirectoryPath;
            foreach (var file in Directory.EnumerateFiles(directoryPath, "*.json"))
            {
                var content = File.ReadAllText(file);
                var deckInfo = JsonUtility.FromJson<DeckInfo>(content);
                yield return infoConverter.Convert(deckInfo);
            }
        }
    }
}
