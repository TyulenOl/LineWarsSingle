using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class VenomEffect<TNode, TEdge, TUnit> 
        : TemporaryEffect<TNode, TEdge, TUnit>, IPowerEffect
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private int venomPower;
        public event Action<IPowerEffect, int, int> PowerChanged;

        public VenomEffect(TUnit targetUnit, int rounds, int venomPower) : base(targetUnit, rounds)
        {
            this.venomPower = venomPower;
        }
        public override EffectType EffectType => EffectType.Venom;

        public int Power => venomPower;

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
            unit.DealDamageThroughArmor(venomPower);
        }

        public void Stack(IStackableEffect effect)
        {
            if (effect is not VenomEffect<TNode, TEdge, TUnit> venomEffect)
            {
                throw new ArgumentException("Can't stack this effect");
            }
            Rounds += venomEffect.Rounds;
            var prevPower = venomPower;
            venomPower = Mathf.Max(venomPower, venomEffect.venomPower);
            PowerChanged?.Invoke(venomEffect, prevPower, venomPower);
        }

        public bool CanStack(IStackableEffect effect)
        {
            return effect is VenomEffect<TNode, TEdge, TUnit>;
        }
    }
}
