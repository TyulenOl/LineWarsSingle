﻿using System;
using UnityEngine;

namespace LineWars.Controllers
{
    [Serializable]
    public class GraphCreatorSettings
    {
        [field: SerializeField] public Vector2 AreaSize { get; set; } = new(20, 40);
        [field: SerializeField, Min(0)] public float Paddings { get; set; } = 0.3f;
        [field: SerializeField] public float PowerOfConnection { get; set; } = 1;
        [field: SerializeField, Range(0, 0.1f)] public float MultiplierOfConnection { get; set; } = 0.006f;
        [field: SerializeField] public float PowerOfRepulsion { get; set; } = 2;
        [field: SerializeField, Range(0, 100)] public float MultiplierOfRepulsion { get; set; } = 1;
        [field: SerializeField, Min(0)] public int NodesCount { get; set; } = 10;
        [field: SerializeField] public Vector2Int EdgesRange { get; set; } = new(2, 4);
    }
}