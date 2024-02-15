using System;

namespace LineWars.Model
{
    public class RoundsTemporaryEffectDecorator<TNode, TEdge, TUnit>: 
        Effect<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private readonly Effect<TNode, TEdge, TUnit> innerEffect;
        private int roundsCount;

        public int InitialRounds { get; private set; }
        public override bool IsActive => innerEffect.IsActive;
        public RoundsTemporaryEffectDecorator(
            TUnit unit, 
            Effect<TNode, TEdge, TUnit> innerEffect, 
            int roundsCount): base(unit)
        {
            if (roundsCount <= 0)
                throw new ArgumentException(nameof(roundsCount));
            if (innerEffect.HasCharacteristic(EffectCharecteristicType.Duration))
                throw new ArgumentException("Don't use this decorator for Temporary Effects!");
            this.innerEffect = innerEffect;
            this.roundsCount = roundsCount;
            InitialRounds = roundsCount;
            characteristics[EffectCharecteristicType.Duration] = () => this.roundsCount;
            isActive = innerEffect.IsActive;         
        }

        public override EffectType EffectType => innerEffect.EffectType;

        public int Rounds => roundsCount;

        public override void ExecuteOnEnter()
        {
            TargetUnit.UnitReplenished += TargetUnitOnUnitReplenished;
            innerEffect.CharacteristicsChanged += OnInnerCharacteristicsChanged;
            innerEffect.ActiveChanged += InvokeActiveChanged;
            innerEffect.ExecuteOnEnter();
        }

        private void TargetUnitOnUnitReplenished(TUnit unit)
        {
            var prevRounds = roundsCount;
            roundsCount--;
            InvokeCharacteristicsChanged(
                this,
                EffectCharecteristicType.Duration,
                prevRounds,
                roundsCount);
            if (roundsCount <= 0)
            {
                TargetUnit.UnitReplenished -= TargetUnitOnUnitReplenished;
                TargetUnit.RemoveEffect(this);
            }
        }

        public override void ExecuteOnExit()
        {
            innerEffect.ExecuteOnExit();
            innerEffect.CharacteristicsChanged -= OnInnerCharacteristicsChanged;
            innerEffect.ActiveChanged -= InvokeActiveChanged;
        }

        public override int GetCharacteristic(EffectCharecteristicType type)
        {
            if(type == EffectCharecteristicType.Duration)
                return base.GetCharacteristic(type);
            return innerEffect.GetCharacteristic(type);
        }

        public override bool HasCharacteristic(EffectCharecteristicType type)
        {
            if (type == EffectCharecteristicType.Duration)
                return base.HasCharacteristic(type);
            return innerEffect.HasCharacteristic(type);
        }

        private void OnInnerCharacteristicsChanged(
            Effect<TNode, TEdge, TUnit> effect, 
            EffectCharecteristicType type, 
            int prevValue,
            int newValue)
        {
            InvokeCharacteristicsChanged(effect, type, prevValue, newValue);    
        }
    }
}