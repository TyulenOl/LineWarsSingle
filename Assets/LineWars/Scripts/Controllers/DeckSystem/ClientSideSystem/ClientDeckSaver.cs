using UnityEngine;
using System.IO;

namespace LineWars.Model
{
    public class ClientDeckSaver : SaverConvertDecorator<Deck, DeckInfo>
    {
        public ClientDeckSaver(
            ISaver<DeckInfo> innerSaver,
            IConverter<Deck, DeckInfo> converter) : base(innerSaver, converter)
        {
        }
    }
}
