using System;
using System.Collections.Generic;

namespace LineWars.Model
{
    public class AuraPowerBuffEffect<TNode, TEdge, TUnit> : Effect<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private List<TUnit> buffedUnits;

        public AuraPowerBuffEffect(TUnit unit) : base(unit)
        {
            unit.
        }

        public override void ExecuteOnEnter()
        {
            
        }

        public override void ExecuteOnExit()
        {
            
        }

        public override void ExecuteOnReplenish()
        {
            
        }
    }
}
