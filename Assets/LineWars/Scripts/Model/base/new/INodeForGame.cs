using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public interface INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> :
        INumbered,
        IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>,
        ITarget,
        INode<TNode, TEdge>
    
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TPlayer: class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TNation: class, INation<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        #endregion 
    {
        public int Visibility { get; }
        public int ValueOfHidden { get; }

        public TUnit LeftUnit { get; set; }
        public TUnit RightUnit { get; set; }

        public bool LeftIsFree => LeftUnit == null;
        public bool RightIsFree => RightUnit == null;

        public bool AllIsFree => LeftIsFree && RightIsFree;
        public bool AnyIsFree => LeftIsFree || RightIsFree;
        public bool IsBase { get; }
    }
}