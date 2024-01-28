using LineWars.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineWars.Model
{
    public class FightingSpiritEffect<TNode, TEdge, TUnit> : Effect<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private int powerBonus;
        public override EffectType EffectType => EffectType.FightingSpirit;
        public FightingSpiritEffect(TUnit targetUnit) : base(targetUnit)
        {
        }


        public override void ExecuteOnEnter()
        {
            throw new NotImplementedException();
        }

        public override void ExecuteOnExit()
        {
            throw new NotImplementedException();
        }
    }
}
