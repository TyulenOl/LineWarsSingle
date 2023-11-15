using LineWars.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class PlayerMoveBan : MonoBehaviour
    {
        [field: SerializeField] public List<Spawn> BannedSpawns { get; private set; }
    }
}
