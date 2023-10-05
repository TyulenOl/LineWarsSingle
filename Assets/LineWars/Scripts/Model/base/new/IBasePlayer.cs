using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public interface IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer>
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer>
        #endregion

    {
        public int Income { get; set; }
        public int CurrentMoney { get; set; }
        public TNode Base { get; }
        public PlayerRules Rules { get;}
        public IReadOnlyCollection<TOwned> OwnedObjects { get; }
        
        public void AddOwned([NotNull] TOwned owned);
        public void RemoveOwned([NotNull] TOwned owned);
        
        public bool CanSpawnPreset(UnitBuyPreset preset);
        public void SpawnPreset(UnitBuyPreset preset);
    }
}