using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using LineWars;
using LineWars.Model;
using UnityEngine;

public class DrawHelperInstance : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<LootType, Sprite> lootTypeToSprite;
    [SerializeField] private SerializedDictionary<Rarity, Color> rarityToColor;
    [SerializeField] private SerializedDictionary<MissionStatus, Sprite> missionStatusToIcon;
    [SerializeField] private SerializedDictionary<CommandType, Sprite> commandTypeToIcon;


    public IReadOnlyDictionary<MissionStatus, Sprite> MissionStatusToIcon => missionStatusToIcon;
    public IReadOnlyDictionary<CommandType, Sprite> CommandTypeToIcon => commandTypeToIcon;
    public IReadOnlyDictionary<LootType, Sprite> LootTypeToSprite => lootTypeToSprite;
    public IReadOnlyDictionary<Rarity, Color> RarityToColor => rarityToColor;
}