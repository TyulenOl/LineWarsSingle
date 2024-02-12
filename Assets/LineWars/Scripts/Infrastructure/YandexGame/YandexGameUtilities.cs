using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using UnityEngine;
using YG;

namespace LineWars
{
    public static class YandexGameUtilities
    {
        // <----------------------------YG->Info------------------------------>
        public static GameInfo UpdateInfo(this GameInfo gameInfo, SavesYG savesYg)
        {
            if (savesYg == null)
            {
                Debug.LogError($"{nameof(savesYg)} is null!");
                return gameInfo;
            }

            // gameInfo.Decks = savesYg.Decks
            //     ?.Convert()
            //     .ToDictionary(deckInfo => deckInfo.Id, deckInfo => deckInfo);
            // gameInfo.UserInfo = savesYg.UserInfoYg
            //     ?.Convert();
            // gameInfo.Missions = savesYg.Missions
            //     ?.Convert()
            //     .ToDictionary(missionInfo => missionInfo.MissionId, missionInfo => missionInfo);

            gameInfo.Decks = savesYg.Decks
                ?.ToDictionary(deckInfo => deckInfo.Id, deckInfo => deckInfo);
            gameInfo.UserInfo = savesYg.UserInfo;
            gameInfo.Missions = savesYg.Missions
                ?.ToDictionary(missionInfo => missionInfo.MissionId, missionInfo => missionInfo);
            
            return gameInfo;
        }

        // public static BlessingId Convert(this BlessingIdYG yg)
        // {
        //     return new BlessingId
        //     (
        //         (BlessingType) yg.Type,
        //         (Rarity) yg.Rarity
        //     );
        // }
        //
        // public static IEnumerable<DeckInfo> Convert(this IEnumerable<DeckInfoYG> yg)
        // {
        //     if (yg == null)
        //         return Enumerable.Empty<DeckInfo>();
        //     return yg
        //         .Where(deck => deck != null)
        //         .Select(deck => new DeckInfo()
        //         {
        //             Id = deck.Id,
        //             Name = "Default",
        //             Cards = deck.CardsId.Select(id => new DeckCardInfo {CardId = id}).ToArray()
        //         });
        // }
        //
        // public static IEnumerable<MissionInfo> Convert(this IEnumerable<MissionInfoYG> yg)
        // {
        //     if (yg == null)
        //         return Enumerable.Empty<MissionInfo>();
        //
        //     return yg
        //         .Where(mission => mission != null)
        //         .Select(mission => new MissionInfo()
        //         {
        //             MissionId = mission.Id,
        //             MissionStatus = (MissionStatus) mission.MissionStatus
        //         });
        // }
        //
        // public static UserInfo Convert(this UserInfoYG yg)
        // {
        //     if (yg == null)
        //         return null;
        //
        //     return new UserInfo()
        //     {
        //         //численные характеристики
        //         Diamonds = yg.Diamonds,
        //         Gold = yg.Gold,
        //         UpgradeCards = yg.UpgradeCards,
        //         PassingGameModes = yg.PassingGameModes,
        //         DefaultBlessingsIsAdded = yg.DefaultBlessingsIsAdded,
        //
        //         //Списки
        //         UnlockedCards = yg.UnlockedCards
        //             ?.ToList(),
        //         SelectedBlessings = yg.SelectedBlessings
        //             ?.Select(Convert)
        //             .ToList(),
        //
        //         //Словари
        //         Blessings = yg.Blessings
        //             ?.ToSerializedDictionary(
        //                 blessing => blessing.Id.Convert(),
        //                 blessing => blessing.Count),
        //         CardLevels = yg.CardLevels
        //             ?.ToSerializedDictionary(level => level.Id, level => level.Level),
        //         LootBoxes = yg.LootBoxes
        //             ?.ToSerializedDictionary(
        //                 box => (LootBoxType) box.Id,
        //                 box => box.Count)
        //     };
        // }

        //<----------------------------Info->YG------------------------------>

        public static SavesYG UpdateSave(this SavesYG savesYg, GameInfo gameInfo)
        {
            if (gameInfo == null)
            {
                Debug.LogError($"{nameof(gameInfo)} is null!");
                return savesYg;
            }

            // if (gameInfo.Decks == null)
            //     Debug.LogError($"{nameof(gameInfo.Decks)} is null!");
            // if (gameInfo.Missions == null)
            //     Debug.LogError($"{nameof(gameInfo.Missions)} is null!");
            // if (gameInfo.UserInfo == null)
            //     Debug.LogError($"{nameof(gameInfo.UserInfo)} is null!");

            // savesYg.Decks = gameInfo.Decks?.Select(pair => pair.Value).Convert().ToArray();
            // savesYg.Missions = gameInfo.Missions?.Select(pair => pair.Value).Convert().ToArray();
            // savesYg.UserInfo = gameInfo.UserInfo?.Convert();

            savesYg.Decks = gameInfo.Decks?.Select(pair => pair.Value).ToArray();
            savesYg.Missions = gameInfo.Missions?.Select(pair => pair.Value).ToArray();
            savesYg.UserInfo = gameInfo.UserInfo;
            
            return savesYg;
        }

        // public static BlessingIdYG Convert(this BlessingId info)
        // {
        //     return new BlessingIdYG()
        //     {
        //         Rarity = (int) info.Rarity,
        //         Type = (int) info.BlessingType,
        //     };
        // }
        //
        // public static IEnumerable<DeckInfoYG> Convert(this IEnumerable<DeckInfo> info)
        // {
        //     if (info == null)
        //         return Enumerable.Empty<DeckInfoYG>();
        //
        //     return info
        //         .Where(deckInfo => deckInfo != null)
        //         .Select(deckInfo => new DeckInfoYG()
        //         {
        //             Id = deckInfo.Id,
        //             CardsId = deckInfo.Cards
        //                 .Where(cardInfo => cardInfo != null)
        //                 .Select(cardInfo => cardInfo.CardId).ToArray()
        //         });
        // }
        //
        // public static IEnumerable<MissionInfoYG> Convert(this IEnumerable<MissionInfo> info)
        // {
        //     if (info == null)
        //         return Enumerable.Empty<MissionInfoYG>();
        //
        //     return info
        //         .Where(missionInfo => missionInfo != null)
        //         .Select(missionInfo => new MissionInfoYG()
        //         {
        //             Id = missionInfo.MissionId,
        //             MissionStatus = (int) missionInfo.MissionStatus
        //         });
        // }
        //
        // public static UserInfoYG Convert(this UserInfo info)
        // {
        //     if (info == null)
        //     {
        //         Debug.LogError($"{nameof(UserInfo)} is null!");
        //         return null;
        //     }
        //     
        //     if (info.UnlockedCards == null)
        //         Debug.LogError($"{nameof(info.UnlockedCards)} is null!");
        //     if (info.Blessings == null)
        //         Debug.LogError($"{nameof(info.Blessings)} is null!");
        //     if (info.CardLevels== null)
        //         Debug.LogError($"{nameof(info.CardLevels)} is null!");
        //     if (info.SelectedBlessings== null)
        //         Debug.LogError($"{nameof(info.SelectedBlessings)} is null!");
        //     if (info.LootBoxes== null)
        //         Debug.LogError($"{nameof(info.LootBoxes)} is null!");
        //     
        //     return new UserInfoYG()
        //     {
        //         //численные характеристики
        //         Diamonds = info.Diamonds,
        //         Gold = info.Gold,
        //         UpgradeCards = info.UpgradeCards,
        //         DefaultBlessingsIsAdded = info.DefaultBlessingsIsAdded,
        //
        //         //списки
        //         UnlockedCards = info.UnlockedCards?.ToArray(),
        //         SelectedBlessings = info.SelectedBlessings?.Select(Convert).ToArray(),
        //
        //         //словари
        //         Blessings = info.Blessings
        //             ?.Select(x => new BlessingCountYG()
        //             {
        //                 Id = x.Key.Convert(),
        //                 Count = x.Value,
        //             })
        //             .ToArray(),
        //         CardLevels = info.CardLevels
        //             ?.Select(x => new CardsLevelYG()
        //             {
        //                 Id = x.Key,
        //                 Level = x.Value
        //             })
        //             .ToArray(),
        //         LootBoxes = info.LootBoxes
        //             ?.Select(x => new LootBoxYG()
        //             {
        //                 Id = (int) x.Key,
        //                 Count = x.Value
        //             }).ToArray()
        //     };
        // }
    }
}