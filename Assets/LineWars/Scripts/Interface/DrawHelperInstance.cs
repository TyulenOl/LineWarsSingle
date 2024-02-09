using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using LineWars;
using LineWars.Model;
using UnityEngine;

public class DrawHelperInstance : MonoBehaviour
{
    [SerializeField] private Sprite goldSprite;

    [SerializeField] private SerializedDictionary<LootType, Color> lootTypeToColor;
    [SerializeField] private SerializedDictionary<LootType, Sprite> lootTypeToSprite;
    [SerializeField] private SerializedDictionary<Rarity, Color> rarityToColor;
    [SerializeField] private SerializedDictionary<MissionStatus, Sprite> missionStatusToIcon;
    [SerializeField] private SerializedDictionary<CommandType, CommandTypeInfo> commandTypeToIcon;
    [SerializeField] private SerializedDictionary<Rarity, GameObject> rarityToShopCardBg;
    [SerializeField] private SerializedDictionary<GradientType, Gradient> typeToGradient;

    public IReadOnlyDictionary<LootType, Color> LootTypeToColor => lootTypeToColor;
    public IReadOnlyDictionary<MissionStatus, Sprite> MissionStatusToIcon => missionStatusToIcon;
    public IReadOnlyDictionary<CommandType, CommandTypeInfo> CommandTypeToIcon => commandTypeToIcon;
    public IReadOnlyDictionary<LootType, Sprite> LootTypeToSprite => lootTypeToSprite;
    public IReadOnlyDictionary<Rarity, Color> RarityToColor => rarityToColor;
    public IReadOnlyDictionary<Rarity, GameObject> RarityToShopCardBg => rarityToShopCardBg;
    public IReadOnlyDictionary<GradientType, Gradient> TypeToGradient => typeToGradient;

    public Sprite GoldSprite => goldSprite;
}

public enum GradientType
{
    Common,
    Epic,
    Gold,
    Upgrade,
    Diamond
}

[Serializable]
public class CommandTypeInfo
{
    public Sprite icon;
    public Sprite orderIcon;
}