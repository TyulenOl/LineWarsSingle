using Codice.Client.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineWars.Model
{
    public interface IUnitFabric<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public TUnit Spawn(TNode node);
        public bool CanSpawn(TNode node);
    }
}
