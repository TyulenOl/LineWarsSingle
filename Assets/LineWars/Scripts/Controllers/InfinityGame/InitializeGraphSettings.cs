using System;
using UnityEngine;

namespace LineWars.Controllers
{
    [Serializable]
    public class InitializeGraphSettings
    {
        [field: SerializeField] public EdgeRemovalType EdgeRemovalType { get; set; }
        [field: SerializeField] public int GenerationAttempts { get; set; } = 100;
        [field: SerializeField] public int MaxIntersectionsCount { get; set; } = 2;
        [field: SerializeField] public int IterationCountBeforeDeleteEdges { get; set; } = 1000;
        [field: SerializeField] public int IterationCountAfterDeleteEdges { get; set; } = 50;
    }

    public enum EdgeRemovalType
    {
        ByIntersectionsCount,
        ByEdgeLength,
    }
}