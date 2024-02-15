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
            characteristics[EffectCharecteristicType.Power] = () => this.firePower;
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
            var prevPower = fireEffect.firePower;
            firePower = Mathf.Max(firePower, fireEffect.firePower);
            InvokeCharacteristicsChanged(this,
                EffectCharecteristicType.Power,
                prevPower,
                firePower);
        }

        public bool CanStack(IStackableEffect effect)
        {
            return effect is FireEffect<TNode, TEdge, TUnit>;
        }
    }
}
