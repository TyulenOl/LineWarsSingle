using System;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    [Serializable]
    public class InitialNodeInfo
    {
        [field: SerializeField] public bool IsSpawn { get; set; }
        [field: SerializeField] public int NodeRadius { get; set; }
        [field: SerializeField] public UnitType LeftUnit { get; set; }
        [field: SerializeField] public UnitType RightUnit { get; set; }
    }
}