using System;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    [Serializable]
    public class PlayersSettings
    {
        [field: SerializeField] public Player PlayerPrefab { get; set; }
        [field: SerializeField] public GameObject EnemyPrefab { get; set; }
    }
}