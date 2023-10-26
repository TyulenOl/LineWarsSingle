using System;

namespace LineWars.Model
{
    public interface IRLBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer> :
        IUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>, ISimpleAction

        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion
    {
        bool IsBlocked { get; }
        public event Action<bool, bool> CanBlockChanged;
        
        public bool CanBlock();
        public void EnableBlock();
    }
}