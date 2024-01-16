using System;
using UnityEngine;

namespace LineWars.Controllers
{
    [Serializable]
    public class InfinityGameSettings
    {
        [field: SerializeField] public InitializeGraphSettings InitializeGraphSettings { get; set; }
        [field: SerializeField] public GraphCreatorSettings GraphCreatorSettings { get; set; }
        [field: SerializeField] public PlayersSettings PlayersSettings { get; set; }
    }
}