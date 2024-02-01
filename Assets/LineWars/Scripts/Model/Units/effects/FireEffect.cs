using System;
using UnityEngine;

namespace LineWars.Model
{
    public class FireEffect<TNode, TEdge, TUnit> : 
        TemporaryEffect<TNode, TEdge, TUnit>,
        IStackableEffect
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private int firePower;
        public FireEffect(TUnit targetUnit, int rounds, int firePower) : base(targetUnit, rounds)
        {
            this.firePower = firePower;
        }
        public override EffectType EffectType => EffectType.Fire;

        public override void ExecuteOnEnter()
        {
            base.ExecuteOnEnter();
            TargetUnit.UnitReplenished += OnReplenish;
        }

        public override void ExecuteOnExit()
        {
            base.ExecuteOnExit();
            TargetUnit.UnitReplenished -= OnReplenish;
        }

        private void OnReplenish(TUnit unit)
        {
            unit.DealDamageThroughArmor(firePower);
        }

        public void Stack(IStackableEffect effect)
        {
            if (effect is not FireEffect<TNode, TEdge, TUnit> fireEffect)
            {
                throw new ArgumentException("Can't stack this effect");
            }
            Rounds += fireEffect.Rounds;
            firePower = Mathf.Max(firePower, fireEffect.firePower);
        }

        public bool CanStack(IStackableEffect effect)
        {
            if (effect is not FireEffect<TNode, TEdge, TUnit>)
                return false;
            return true;
        }
    }
}
