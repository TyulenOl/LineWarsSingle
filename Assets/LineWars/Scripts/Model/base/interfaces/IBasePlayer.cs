using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public interface IBasePlayer
    {
        public int Id { get; }
        
        public bool CanSpawnPreset(UnitBuyPreset preset);
        public void SpawnPreset(UnitBuyPreset preset);
    }
}