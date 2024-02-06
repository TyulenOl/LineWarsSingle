using System;
using System.Collections.Generic;
using LineWars;
using LineWars.Controllers;
using LineWars.Interface;
using LineWars.Model;
using UnityEngine;

public static class DrawHelper
{
    private static DrawHelperInstance Instance => GameRoot.Instance.DrawHelper;
    public static IReadOnlyDictionary<LootType, Sprite> LootTypeToSprite => Instance.LootTypeToSprite;
    public static IReadOnlyDictionary<Rarity, Color> RarityToColor => Instance.RarityToColor;
    public static IReadOnlyDictionary<Rarity, GameObject> RarityToShopCardBg => Instance.RarityToShopCardBg;
    public static IReadOnlyDictionary<GradientType, Gradient> TypeToGradient => Instance.TypeToGradient;
    public static Sprite GoldSprite => Instance.GoldSprite;
    
    public static FullBlessingReDrawInfo GetBlessingReDrawInfoByBlessingId(BlessingId blessingId)
    {
        return new FullBlessingReDrawInfo
        (
            GetBlessingNameByBlessingID(blessingId),
            GetBlessingDescription(blessingId),
            GetBlessingBgColor(blessingId),
            GetSpriteByBlessingID(blessingId),
            GameRoot.Instance.UserController.GlobalBlessingsPull[blessingId]
        );
    }

    public static Color GetBlessingBgColor(BlessingId blessingId)
    {
        switch (blessingId.Rarity)
        {
            case Rarity.Common:
                return new Color32(120, 120, 120,255);
            case Rarity.Rare:
                return new Color32(226, 43, 18,255);
            case Rarity.Legendary:
                return new Color32(143, 0, 255,255);
            default:
                return new Color32(120, 120, 120,255);
        }
    }
    
    public static Sprite GetSpriteByBlessingID(BlessingId blessingId)
    {
        return GameRoot.Instance.BlessingStorage.TryGetValue(blessingId, out var value)
            ? value.Icon
            : null;
    }

    public static string GetBlessingTypeName(BlessingType blessingType)
    {
        return blessingType switch
        {
            BlessingType.Perun => "Благословление Перуна",
            BlessingType.Svarog => "Благословление Сварога",
            BlessingType.Health => "Благословление Здоровья",
            BlessingType.Stribog => "Благословление Стрибога",
            BlessingType.Acceleration => "Благословление Шустрости",
            BlessingType.Slowdown => "Благословление Сонливости",
            BlessingType.Power => "Благословление Мощи",
            BlessingType.Spawn => "Благословление Призыва",
            BlessingType.Gold => "Благословление Богатства",
            BlessingType.Random => "Благословление Случайности",
            _ => "Если ты видишь это - ты уволен"
        };
    }
    
    public static string GetBlessingNameByBlessingID(BlessingId blessingId)
    {
        return GetBlessingTypeName(blessingId.BlessingType);
    }
    public static string GetBlessingDescription(BlessingId blessingId)
    {
        return GameRoot.Instance.BlessingStorage.TryGetValue(blessingId, out var value)
            ? value.Description
            : "Привет из мира багов";
    }
    
    public static Sprite GetSpriteByMissionStatus(MissionStatus missionStatus)
    {
        if (Instance.MissionStatusToIcon.TryGetValue(missionStatus, out var sprite))
            return sprite;
        return null;
    }
    
    public static Sprite GetSpriteByCommandType(CommandType commandType)
    {
        if (Instance.CommandTypeToIcon.TryGetValue(commandType, out var sprite))
            return sprite;
        return null;
    }
    

    public static ActionReDrawInfo GetReDrawInfoByCommandType(CommandType commandType)
    {
        switch (commandType)
        {
            case CommandType.MeleeAttack:
                return new ActionReDrawInfo(
                    GetSpriteByCommandType(commandType),
                    "Ближний бой",
                    "Рус атакует выбранного ящера на соседней клетке, нанося ему урон, равный силе духа, но получая обратный урон, который зависит от силы ящера." +
                    "Если после ближнего sбоя ящер погибает, рус захватывает точку, перемещаясь на нее.");
            case CommandType.SacrificePerun:
                return new ActionReDrawInfo(
                    GetSpriteByCommandType(commandType),
                    
                    "Жертва перуну",
                    
                    "Рус приносит себя в жертву Перуну. После этого укажите на любую точку на карте - Перун пошлет туда молнию. Если на точке есть ящеры," +
                    " она нанесет им урон, равный сумме здоровья и брони юнита, принесшего жертву. Если ящеров двое на точке - урон распределится");
            case CommandType.VodaBajkalskaya:
                return new ActionReDrawInfo(
                    GetSpriteByCommandType(commandType),
                    
                    "Выпить воды байкальской",
                    
                    "Рус пьет водичку байкальскую, тратит все свои очки действия и восстанавливает 2 еденицы здоровья.");
            case CommandType.Block:
                return new ActionReDrawInfo(
                    GetSpriteByCommandType(commandType),
                    
                    "Встать в защиту",
                    
                    "Рус тратит переводит свои оставшиеся очки действия в броню. Броня пропадает в конце следующего раунда.");
            case CommandType.Fire:
                return new ActionReDrawInfo(
                    GetSpriteByCommandType(commandType),
                    
                    "Выстрел",
                    
                    "Рус стреляет по выбранному ящеру на соседней клетке, нанося урон равный своей силе духа, ящер не наносит ответный урон. Даже если после выстрела на точке погибли все ящеры -" +
                    " рус не встает на точку.");
            case CommandType.ShotUnit:
                return new ActionReDrawInfo(
                    GetSpriteByCommandType(commandType),

                    "Могучий Бросок",

                    "Рус берет юнита на соседней клетке и кидает его на выбранную клетку. " +
                    "Если на ней располагаются другие юниты, то и брошенный и стоящий юнит получают урон, равный ХП своего противника." +
                    "Если на клетке стоят два юнита, урон распределяется пополам.");
            case CommandType.UpArmor:
                return new ActionReDrawInfo(
                    GetSpriteByCommandType(commandType),

                    "Опьяняющая настойка",

                    "Рус поднимает выбранному русу на соседней клетке броню на силу духа юнита.");
            case CommandType.Ram:
                return new ActionReDrawInfo(
                    GetSpriteByCommandType(commandType),

                    "Таран",

                    "Рус наносит ящерам на выбранной соседней клетке урон, равный силе духа юнита, " +
                    "и выталкивает их в соседнюю клетку ящеров. Если такой нет, то ящеры умирают. ");
            case CommandType.BlowWithSwing:
                return new ActionReDrawInfo(
                    GetSpriteByCommandType(commandType),

                    "Размах топором",

                    "Рус наносит всем ящерам на соседних клетках урон, равный своей силе духа.");
            case CommandType.Stun:
                return new ActionReDrawInfo(
                    GetSpriteByCommandType(commandType),

                    "Оглушение",

                    "Рус понижает выбранному ящеру на соседней клетке очки действия на свою силу");
            case CommandType.HealingAttack:
                return new ActionReDrawInfo(
                    GetSpriteByCommandType(commandType),

                    "Голодный Укус",

                    "Рус наносит урон выбранному ящеру на соседней клетке, равный своей силе духа. " +
                    "Рус восстанавливает себе броню на свою силу духа.");
            case CommandType.TargetPowerBasedAttack:
                return new ActionReDrawInfo(
                    GetSpriteByCommandType(commandType),

                    "Гипноз Меча",

                    "Рус наносит урон выбранному ящеру на соседней клетке, равный сумме своей силы духа и силы духа ящера.");
            case CommandType.PowerBasedHeal:
                return new ActionReDrawInfo(
                    GetSpriteByCommandType(commandType),

                    "Кваску?",

                    "Рус поднимает выбранному русу на соседней клетке ХП на свою силу духа.");

        }

        return null;
    }

    public static string GetOnMissionButtonTextByMissionStatus(MissionStatus missionStatus)
    {
        switch (missionStatus)
        {
            case MissionStatus.Failed:
                return "Реванш";
            case MissionStatus.Unlocked:
                return "В бой";
            case MissionStatus.Completed:
                return "Заново";
            default:
                return "В бой";
        }
    }
}