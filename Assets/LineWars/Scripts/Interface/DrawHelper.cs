using System;
using System.Collections;
using System.Collections.Generic;
using LineWars;
using LineWars.Controllers;
using LineWars.Interface;
using LineWars.Model;
using UnityEngine;

public static class DrawHelper
{
    public static Sprite GetSpriteByCharacteristicType(CharacteristicType characteristicType)
    {
        switch (characteristicType)
        {
            case CharacteristicType.MeleeAttack:
                return Resources.Load<Sprite>("UI/Sorokin/Icons/Attack");
            case CharacteristicType.Armor:
                return Resources.Load<Sprite>("UI/Sorokin/Icons/Block");
            case CharacteristicType.Hp:
                return Resources.Load<Sprite>("UI/Sorokin/Icons/Heal");
            case CharacteristicType.ActionPoints:
                return Resources.Load<Sprite>("UI/Sorokin/Icons/AP");
            default: return Resources.Load<Sprite>("UI/Sorokin/Icons/Heal");
        }
    }


    public static BlessingReDrawInfo GetBlessingReDrawInfoByBlessingId(BlessingId blessingId)
    {
        return new BlessingReDrawInfo
        (
            GetBlessingNameByBlessingID(blessingId),
            GetBlessingDescription(blessingId),
            GetBlessingBgColor(blessingId),
            GetSpriteByBlessingID(blessingId),
            GameRoot.Instance.BlessingsController.GetBlessingsCount(blessingId)
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
            : Resources.Load<Sprite>("UI/Sorokin/Icons/AP");
    }

    public static string GetBlessingTypeName(BlessingType blessingType)
    {
        switch (blessingType)
        {
            case BlessingType.Perun:
                return "Сила перуна";
            case BlessingType.Svarog:
                return "Сила сварога";
            case BlessingType.Health:
                return "Лечение";
            case BlessingType.Stribog:
                return "Сила стрибога";
            case BlessingType.Acceleration:
                return "Ускорение";
            case BlessingType.Slowdown:
                return "Замедление";
            case BlessingType.Power:
                return "Сила";
            case BlessingType.Spawn:
                return "Призыва";
            case BlessingType.Gold:
                return "Золота";
            case BlessingType.Random:
                return "Случайное";
            default:
                return "Если ты видишь это - ты уволен";
        }
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
        
        // switch (blessingId.BlessingType)
        // {
        //     case BlessingType.Perun:
        //         return "Наносит всем врагам урон";
        //     case BlessingType.Svarog:
        //         return "Увеличивает броню всех союзников";
        //     case BlessingType.Health:
        //         return "Восстанавливает здоровье всех союзников";
        //     case BlessingType.Stribog:
        //         return "Рассеивает туман войны по всей карте на огранниченное число раундов";
        //     case BlessingType.Acceleration:
        //         return "Восстанавливает часть очков действия всем союзникам";
        //     case BlessingType.Slowdown:
        //         return "Уменьшает очки действия всех врагов";
        //     case BlessingType.Power:
        //         return "Увеличивает силу всем союзников на огранниченное число раундов";
        //     case BlessingType.Spawn:
        //         return "Призывает различных существ на вашей базе";
        //     case BlessingType.Gold:
        //         return "Осыпает вас золотом";
        //     case BlessingType.Random:
        //         return "Случайное благословение случайной редкости";
        //     default:
        //         return "Привет из мира багов";
        // }
    }
    
    public static Sprite GetSpriteByMissionStatus(MissionStatus missionStatus)
    {
        switch (missionStatus)
        {
            case MissionStatus.Locked:
                return Resources.Load<Sprite>("UI/Sorokin/Panels/CompanyChoose/LockedMission");
            case MissionStatus.Completed:
                return Resources.Load<Sprite>("UI/Sorokin/Panels/CompanyChoose/CompletedMission");
            case MissionStatus.Unlocked:
                return Resources.Load<Sprite>("UI/Sorokin/Panels/CompanyChoose/OpenMission");
            case MissionStatus.Failed:
                return Resources.Load<Sprite>("UI/Sorokin/Panels/CompanyChoose/DefeatedMission");
            default: return Resources.Load<Sprite>("UI/Sorokin/Panels/CompanyChoose/LockedMission");
        }
    }
    
    public static Sprite GetSpriteByCommandType(CommandType commandType)
    {
        switch (commandType)
        {
            case CommandType.MeleeAttack:
                return Resources.Load<Sprite>("UI/Sorokin/Icons/AttackOrder");
            case CommandType.Ram:
                return Resources.Load<Sprite>("UI/Sorokin/Icons/RamOrder");
            case CommandType.ShotUnit:
                return Resources.Load<Sprite>("UI/Sorokin/Icons/ThrowOrder");
            case CommandType.BlowWithSwing:
                return Resources.Load<Sprite>("UI/Sorokin/Icons/BlowOrder");
            case CommandType.Heal:
                return Resources.Load<Sprite>("UI/Sorokin/Icons/Heal");
            case CommandType.SacrificePerun:
                return Resources.Load<Sprite>("UI/Sorokin/Icons/SacryficeOrder");
            case CommandType.Fire:
                return Resources.Load<Sprite>("UI/Sorokin/Icons/DistanseAttackOrder");
            case CommandType.Move:
                return Resources.Load<Sprite>("UI/Sorokin/Icons/MoveOrder");
            case CommandType.Build:
                return Resources.Load<Sprite>("UI/Icons/Upgrade");
            case CommandType.HealingAttack:
                return Resources.Load<Sprite>("UI/Sorokin/Icons/KusOrder");
            case CommandType.UpArmor:
                return Resources.Load<Sprite>("UI/Sorokin/Icons/UpArmorOrder");
            case CommandType.Stun:
                return Resources.Load<Sprite>("UI/Sorokin/Icons/BamOrder");
            case CommandType.Block:
                return Resources.Load<Sprite>("UI/Sorokin/Icons/BlockOrder");
            case CommandType.VodaBajkalskaya:
                return Resources.Load<Sprite>("UI/Sorokin/Icons/WaterOrder");
            case CommandType.TargetPowerBasedAttack:
                return Resources.Load<Sprite>("UI/Sorokin/Icons/GuipnosusOrder");
            default: return Resources.Load<Sprite>("UI/Sorokin/Icons/AttackOrder");
        }
    }

    public static string GetPhaseName(PhaseType phaseType)
    {
        switch (phaseType)
        {
            case PhaseType.Artillery:
                return "Артподготовка";
            case PhaseType.Buy:
                return "Подкрепление";
            case PhaseType.Fight:
                return "Боевые действия";
            case PhaseType.Scout:
                return "Разведка";
            default: return "Подготовка";
        }
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


    [Obsolete]
    public static Sprite GetSpriteByUnitType(UnitType characteristicType)
    {
        switch (characteristicType)
        {
            case UnitType.TheRifleMan:
                return Resources.Load<Sprite>("UI/Icons/rifle-type");
            case UnitType.SubmachineGunner:
                return Resources.Load<Sprite>("UI/Icons/autorifle-type");
            case UnitType.Doctor:
                return Resources.Load<Sprite>("UI/Icons/medic-type");
            default: return Resources.Load<Sprite>("UI/Icons/rifle-type");
        }
    }

    public static string GetNameByUnitType(UnitType characteristicType)
    {
        switch (characteristicType)
        {
            case UnitType.TheRifleMan:
                return "Rifleman";
            case UnitType.SubmachineGunner:
                return "Assault";
            case UnitType.Doctor:
                return "Doctor";
            default: return "Солдат";
        }
    }
}

public enum CharacteristicType
{
    MeleeAttack,
    FarAttack,
    Armor,
    Hp,
    ActionPoints
}