using UnityEngine;
using System.IO;

namespace LineWars.Model
{
    public class ClientDeckSaver : ISaver<Deck>
    {
        private IConverter<Deck, DeckInfo> deckConverter;
        public ClientDeckSaver(IConverter<Deck, DeckInfo> deckConverter)
        {
            this.deckConverter = deckConverter;
        }

        public void Save(Deck value, int id)
        {
            var newDeckInfo = deckConverter.Convert(value);
            var deckJson = JsonUtility.ToJson(newDeckInfo);
            var path = ClientDeckInfoSystemUtilities.GetDeckInfoFilePath(id);
            File.WriteAllText(path, deckJson);
        }
    }
}
