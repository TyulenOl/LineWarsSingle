using System;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Controllers
{
    [Serializable]
    public class PlayersSettings
    {
        [SerializeField, ReadOnlyInspector] private string description = "The first one on the list is the player";
        [field: SerializeField, NamedArray("playerPrefab")] public List<PlayerSettings> Players { get; set; }
        
        [field: SerializeField] public GameRefereeCreator GameRefereeCreator { get; set; }
    }
}