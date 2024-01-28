using UnityEngine;
using System.Collections.Generic;

namespace LineWars.Model
{ 
    public interface IDeckCard
    {
        public string Name { get; } 
        public string Description { get; }
        public Sprite Image { get; }
        public Unit Unit { get; }   
        public Rarity Rarity { get; }
        public IReadOnlyList<IReadOnlyCardLevelInfo> Levels { get; }
        public int MaxLevel { get; }
    }
}
