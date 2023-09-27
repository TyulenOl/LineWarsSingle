// using System;
// using System.Collections.Generic;
//
// namespace LineWars.Model
// {
//     public interface IBasePlayer : INumbered, IActor
//     {
//         public Nation MyNation { get; }
//         public int Income { get; set; }
//         public int CurrentMoney { get; set; }
//
//         public IReadOnlyCollection<IOwned> IOwnedObjects { get; }
//
//         public bool CanSpawnPreset(UnitBuyPreset preset);
//         public bool SpawnPreset(UnitBuyPreset preset);
//     }
// }