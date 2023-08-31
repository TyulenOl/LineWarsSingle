using System.Collections;
using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;

public static class DrawHelper
{

    public static Sprite GetSpriteByCharacteristicType(CharacteristicType characteristicType)
    {
        switch (characteristicType)
        {
            case CharacteristicType.MeleeAttack:
                return Resources.Load<Sprite>("UI/Icons/attack");
            case CharacteristicType.Armor:
                return Resources.Load<Sprite>("UI/Icons/armor");
            case CharacteristicType.Hp:
                return Resources.Load<Sprite>("UI/Icons/HP");
            default: return Resources.Load<Sprite>("UI/Icons/HP");
        }
    }
    
    public static Sprite GetSpriteByCommandType(CommandType commandType)
    {
        switch (commandType)
        {
            case CommandType.MeleeAttack:
                return Resources.Load<Sprite>("UI/Icons/attack");
            case CommandType.Heal:
                return Resources.Load<Sprite>("UI/Icons/Hp");
            case CommandType.Fire:
                return Resources.Load<Sprite>("UI/Icons/fire");
            case CommandType.Move:
                return Resources.Load<Sprite>("UI/Icons/target");
            default: return Resources.Load<Sprite>("UI/Icons/HP");
        }
    }

    
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
    Hp
}
