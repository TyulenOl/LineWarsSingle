using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public interface INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> :
            INumbered,
            IOwned<TOwned, TPlayer>,
            ITarget,
            INode<TNode, TEdge>

        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion

    {
        public int Visibility { get; }
        public int ValueOfHidden { get; }

        public TUnit LeftUnit { get; set; }
        public TUnit RightUnit { get; set; }
        public IBuilding Building { get; set; }

        public IEnumerable<TUnit> Units => new[] {LeftUnit, RightUnit}
            .Where(x => x != null)
            .Distinct();

        public bool LeftIsFree => LeftUnit == null;
        public bool RightIsFree => RightUnit == null;

        public bool AllIsFree => LeftIsFree && RightIsFree;
        public bool AnyIsFree => LeftIsFree || RightIsFree;
        public bool IsBase { get; }

        public T Accept<T>(INodeVisitor<T> visitor);
    }
}