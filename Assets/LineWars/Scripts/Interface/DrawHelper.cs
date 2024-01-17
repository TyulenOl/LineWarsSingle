using System;
using System.Collections;
using System.Collections.Generic;
using LineWars;
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

            default: return Resources.Load<Sprite>("UI/Sorokin/Icons/DistanseAttackOrder");
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
                var sprite = GetSpriteByCommandType(commandType);

                var name = "Ближний бой";

                var desription = "Рус атакует ящера, нанося ему урон, равный силе духа. После ближней атаки число очков действия руса приравнивается к нулю";
                
                return new ActionReDrawInfo(sprite, name,desription);
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