using LineWars.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    [Obsolete]
    [DisallowMultipleComponent]
    public class PlayerMoveBan : MonoBehaviour
    {
        [field: SerializeField] public List<Node> BannedNodes { get; private set; }
    }
}
