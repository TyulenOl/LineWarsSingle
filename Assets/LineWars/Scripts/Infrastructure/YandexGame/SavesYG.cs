using LineWars;
using LineWars.Model;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;
        
        // Ваши сохранения

        public UserInfo UserInfo;
        public DeckInfo[] Decks;
        public MissionInfo[] Missions;
        
        
        // ...

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны
        public SavesYG()
        {
        }
    }
    
    // [Serializable]
    // public class UserInfoYG
    // {
    //     public int Diamonds;
    //     public int Gold;
    //     public int[] UnlockedCards;
    //     public int UpgradeCards;
    //     public LootBoxYG[] LootBoxes;
    //     public CardsLevelYG[] CardLevels;
    //     public int PassingGameModes;
    //     
    //     public bool DefaultBlessingsIsAdded;
    //
    //     public BlessingCountYG[] Blessings;
    //     public BlessingIdYG[] SelectedBlessings;
    // }
    //
    // [Serializable]
    // public class LootBoxYG
    // {
    //     public int Id;
    //     public int Count;
    // }
    //
    // [Serializable]
    // public class CardsLevelYG
    // {
    //     public int Id;
    //     public int Level;
    // }
    //
    // [Serializable]
    // public class BlessingCountYG
    // {
    //     public BlessingIdYG Id;
    //     public int Count;
    // }
    //
    // [Serializable]
    // public struct BlessingIdYG
    // {
    //     public int Rarity;
    //     public int Type;
    // }
    //
    // [Serializable]
    // public class DeckInfoYG
    // {
    //     public int Id;
    //     public int[] CardsId;
    // }
    //
    // [Serializable]
    // public class MissionInfoYG
    // {
    //     public int Id;
    //     public int MissionStatus;
    // }
}
