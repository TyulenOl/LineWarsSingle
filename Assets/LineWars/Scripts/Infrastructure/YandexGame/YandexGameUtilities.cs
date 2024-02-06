using System.Collections.Generic;
using System.Linq;
using LineWars.Model;
using YG;

namespace LineWars
{
    public static class YandexGameUtilities
    {
        
        // <----------------------------YG->Info------------------------------>
        public static GameInfo UpdateInfo(this GameInfo gameInfo, SavesYG savesYg)
        {
            gameInfo.Decks = savesYg.Decks
                .Convert()
                .ToDictionary(deckInfo => deckInfo.Id, deckInfo => deckInfo);
            gameInfo.UserInfo = savesYg.UserInfoYg.Convert();
            gameInfo.Missions = savesYg.Missions
                .Convert()
                .ToDictionary(missionInfo => missionInfo.MissionId, missionInfo => missionInfo);
            
            return gameInfo;
        }
        
        public static BlessingId Convert(this BlessingIdYG yg)
        {
            return new BlessingId(
                (BlessingType) yg.Type,
                (Rarity) yg.Rarity);
        }

        public static IEnumerable<DeckInfo> Convert(this IEnumerable<DeckInfoYG> yg)
        {
            return yg.Select(deck => new DeckInfo()
            {
                Id = deck.Id,
                Name = "Default",
                Cards = deck.CardsId.Select(id => new DeckCardInfo {CardId = id}).ToArray()
            });
        }

        public static IEnumerable<MissionInfo> Convert(this IEnumerable<MissionInfoYG> yg)
        {
            return yg.Select(mission => new MissionInfo()
            {
                MissionId = mission.Id,
                MissionStatus = (MissionStatus) mission.MissionStatus
            });
        }

        public static UserInfo Convert(this UserInfoYG yg)
        {
            return new UserInfo()
            {
                //численные характеристики
                Diamonds = yg.Diamonds,
                Gold = yg.Gold,
                UpgradeCards = yg.UpgradeCards,
                PassingGameModes = yg.PassingGameModes,
                DefaultBlessingsIsAdded = yg.DefaultBlessingsIsAdded,

                //Списки
                UnlockedCards = yg.UnlockedCards
                    .ToList(),
                SelectedBlessings = yg.SelectedBlessings
                    .Select(Convert)
                    .ToList(),

                //Словари
                Blessings = yg.Blessings
                    .ToSerializedDictionary(
                        blessing => blessing.Id.Convert(),
                        blessing => blessing.Count),
                CardLevels = yg.CardLevels
                    .ToSerializedDictionary(level => level.Id, level => level.Level),
                LootBoxes = yg.LootBoxes.ToSerializedDictionary(
                    box => (LootBoxType) box.Id,
                    box => box.Count)
            };
        }

        //<----------------------------Info->YG------------------------------>

        public static SavesYG UpdateSave(this SavesYG savesYg, GameInfo gameInfo)
        {
            savesYg.Decks = gameInfo.Decks.Select(pair => pair.Value).Convert().ToArray();
            savesYg.Missions = gameInfo.Missions.Select(pair => pair.Value).Convert().ToArray();
            savesYg.UserInfoYg = gameInfo.UserInfo.Convert();
            
            return savesYg;
        }
        
        public static BlessingIdYG Convert(this BlessingId info)
        {
            return new BlessingIdYG()
            {
                Rarity = (int) info.Rarity,
                Type = (int) info.BlessingType,
            };
        }

        public static IEnumerable<DeckInfoYG> Convert(this IEnumerable<DeckInfo> info)
        {
            return info.Select(deckInfo => new DeckInfoYG()
            {
                Id = deckInfo.Id,
                CardsId = deckInfo.Cards.Select(x => x.CardId).ToArray()
            });
        }

        public static IEnumerable<MissionInfoYG> Convert(this IEnumerable<MissionInfo> info)
        {
            return info.Select(missionInfo => new MissionInfoYG()
            {
                Id = missionInfo.MissionId,
                MissionStatus = (int) missionInfo.MissionStatus
            });
        }

        public static UserInfoYG Convert(this UserInfo info)
        {
            return new UserInfoYG()
            {
                //численные характеристики
                Diamonds = info.Diamonds,
                Gold = info.Gold,
                UpgradeCards = info.UpgradeCards,
                DefaultBlessingsIsAdded = info.DefaultBlessingsIsAdded,

                //списки
                UnlockedCards = info.UnlockedCards.ToArray(),
                SelectedBlessings = info.SelectedBlessings.Select(Convert).ToArray(),

                //словари
                Blessings = info.Blessings
                    .Select(x => new BlessingCountYG()
                    {
                        Id = x.Key.Convert(),
                        Count = x.Value,
                    })
                    .ToArray(),
                CardLevels = info.CardLevels
                    .Select(x => new CardsLevelYG()
                    {
                        Id = x.Key,
                        Level = x.Value
                    })
                    .ToArray(),
                LootBoxes = info.LootBoxes
                    .Select(x => new LootBoxYG()
                    {
                        Id = (int)x.Key,
                        Count = x.Value
                    }).ToArray()
            };
        }
    }
}