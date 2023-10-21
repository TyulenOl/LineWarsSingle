using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public interface IBasePlayer<TOwned, TPlayer> : INumbered
        #region Сonstraints
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TOwned, TPlayer>
        #endregion

    {
        public PhaseExecutorsData PhaseExecutorsData { get; }
        public NationEconomicLogic EconomicLogic { get; }   
        //public Nation Nation { get; }
        public IReadOnlyCollection<TOwned> OwnedObjects { get; }
        
        public void AddOwned([NotNull] TOwned owned);
        public void RemoveOwned([NotNull] TOwned owned);
        
        public bool CanSpawnPreset(UnitBuyPreset preset);
        public void SpawnPreset(UnitBuyPreset preset);
    }
}