using System;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Controllers
{
    [Serializable]
    public class PlayersSettings
    {
        [field: SerializeField, NamedArray("playerPrefab")] public List<PlayerSettings> Players { get; set; }
        
        [field: SerializeField] public GameRefereeCreator GameRefereeCreator { get; set; }
        [field: SerializeField] public Vector2Int BaseNodesIncomeRange { get; set; } = new (3, 10);
    }
}