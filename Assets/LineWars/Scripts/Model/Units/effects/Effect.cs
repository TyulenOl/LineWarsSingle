using System.Collections.Generic;
using System;

namespace LineWars.Model
{
    public abstract class Effect<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public TUnit TargetUnit { get; private set; }
        public abstract EffectType EffectType { get; }
        
        protected Dictionary<EffectCharecteristicType, Func<int>> characteristics;

        public event Action<Effect<TNode, TEdge, TUnit>, bool, bool> ActiveChanged;
        public event Action<Effect<TNode, TEdge, TUnit>, EffectCharecteristicType, int, int> CharacteristicsChanged;

        private bool _isActive = true;
        protected bool isActive
        {
            get => _isActive;
            set
            {
                if(_isActive == value) return;
                var prevValue = _isActive;
                _isActive = value;
                ActiveChanged?.Invoke(this, prevValue, _isActive);
            }
        }

        public virtual bool IsActive => _isActive;

        public Effect(TUnit targetUnit)
        {
            TargetUnit = targetUnit;
            characteristics = new();
        }

        public abstract void ExecuteOnEnter();
        public abstract void ExecuteOnExit();
        public virtual bool HasCharacteristic(EffectCharecteristicType type)
            => characteristics.ContainsKey(type);

        public virtual int GetCharacteristic(EffectCharecteristicType type)
        {
            return characteristics[type]();
        }

        protected void InvokeCharacteristicsChanged(Effect<TNode, TEdge, TUnit> effect,
            EffectCharecteristicType type,
            int prevValue,
            int newValue)
        {
            CharacteristicsChanged?.Invoke(effect, type, prevValue, newValue); 
        }

        protected void InvokeActiveChanged(
            Effect<TNode, TEdge, TUnit> effect,
            bool prevValue,
            bool newValue)
        {
            ActiveChanged?.Invoke(effect, prevValue, newValue);
        }
    }

    public enum EffectCharecteristicType
    {
        Power,
        Duration
    }
}
