using UnityEngine;

namespace LineWars.Model
{
    public class ClientDeckInfoSystemUtilities
    {
        public static string DeckDirectoryPath => Application.persistentDataPath + "/Decks";
        public static string AllDeckInfoFilePath => DeckDirectoryPath + "/AllDeckInfo.json";
        public static string GetDeckInfoFilePath(int deckId)
        {
            return DeckDirectoryPath + $"Deck_{deckId}.json";
        }
    }
}
