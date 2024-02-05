using LineWars.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars
{
    public class IllnessEffect<TNode, TEdge, TUnit> :
        TemporaryEffect<TNode, TEdge, TUnit>,
        IStackableEffect
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private int powerDebuff;
        public IllnessEffect(
            TUnit targetUnit, 
            int rounds,
            int powerDebuff) : base(targetUnit, rounds)
        {
            this.powerDebuff = powerDebuff;
        }

        public override EffectType EffectType => EffectType.Illness;

        public override void ExecuteOnEnter()
        {
            base.ExecuteOnEnter();
            TargetUnit.CurrentPower -= powerDebuff;
        }

        public override void ExecuteOnExit()
        {
            base.ExecuteOnExit();
            TargetUnit.CurrentPower += powerDebuff;
        }

        public bool CanStack(IStackableEffect effect)
        {
            throw new System.NotImplementedException();
        }

        public void Stack(IStackableEffect effect)
        {
            throw new System.NotImplementedException();
        }
    }
}
