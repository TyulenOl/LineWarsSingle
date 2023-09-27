using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public interface IBasePlayer: IReadOnlyBasePlayer
    {
        public new INode Base { get; }
        public new IReadOnlyCollection<IOwned> OwnedObjects { get; }
        
        public void AddOwned([NotNull] IOwned owned);
        public void RemoveOwned([NotNull] IOwned owned);
        public void SpawnPreset(UnitBuyPreset preset);
        public bool CanSpawnPreset(UnitBuyPreset preset);
    }
}