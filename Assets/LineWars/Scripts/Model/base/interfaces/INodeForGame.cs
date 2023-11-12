using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public interface INodeForGame<TNode, TEdge, TUnit> :
        IOwned,
        ITarget,
        INode<TNode, TEdge>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public int Id { get; }
        public int Visibility { get; }
        public int ValueOfHidden { get; }

        public bool IsSpawn { get; }
        public TUnit LeftUnit { get; set; }
        public TUnit RightUnit { get; set; }
        public IBuilding Building { get; set; }

        public IEnumerable<TUnit> Units { get; }

        public bool LeftIsFree => LeftUnit == null;
        public bool RightIsFree => RightUnit == null;

        public bool AllIsFree => LeftIsFree && RightIsFree;
        public bool AnyIsFree => LeftIsFree || RightIsFree;
        public bool IsBase { get; }
    }
}