using System;

namespace LineWars.Model
{
    public class RoundsTemporaryEffectDecorator<TNode, TEdge, TUnit>: 
        Effect<TNode, TEdge, TUnit>, ITemporaryEffect
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private readonly Effect<TNode, TEdge, TUnit> innerEffect;
        private int roundsCount;

        public event Action<ITemporaryEffect, int, int> RoundsChanged;

        public int InitialRounds { get; private set; }

        public RoundsTemporaryEffectDecorator(
            TUnit unit, 
            Effect<TNode, TEdge, TUnit> innerEffect, 
            int roundsCount): base(unit)
        {
            if (roundsCount <= 0)
                throw new ArgumentException(nameof(roundsCount));
            
            this.innerEffect = innerEffect;
            this.roundsCount = roundsCount;
            InitialRounds = roundsCount;

        }

        public override EffectType EffectType => innerEffect.EffectType;


        public int Rounds => roundsCount;

        public override void ExecuteOnEnter()
        {
            TargetUnit.UnitReplenished += TargetUnitOnUnitReplenished;
            innerEffect.ExecuteOnEnter();
        }

        private void TargetUnitOnUnitReplenished(TUnit unit)
        {
            var prevRounds = roundsCount;
            roundsCount--;
            RoundsChanged?.Invoke(this, prevRounds, roundsCount);
            if (roundsCount <= 0)
            {
                TargetUnit.UnitReplenished -= TargetUnitOnUnitReplenished;
                TargetUnit.RemoveEffect(this);
            }
        }

        public override void ExecuteOnExit()
        {
            innerEffect.ExecuteOnExit();
        }
    }
}