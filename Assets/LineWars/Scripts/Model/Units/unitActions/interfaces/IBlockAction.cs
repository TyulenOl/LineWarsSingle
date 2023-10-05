using System;
using System.Diagnostics.CodeAnalysis;


namespace LineWars.Model
{
    public interface IBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer>:
        IUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>
    
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer>
        #endregion 
    {
        bool IsBlocked { get; }
        bool Protection { get; }
        event Action<bool, bool> CanBlockChanged;
        bool CanBlock();
        void EnableBlock();
        bool CanContrAttack([NotNull] TUnit enemy);
        void ContrAttack([NotNull] TUnit enemy);
    }
}