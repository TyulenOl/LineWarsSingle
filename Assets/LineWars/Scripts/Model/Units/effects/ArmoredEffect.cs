using System;

namespace LineWars.Model
{
    public class ArmoredEffect<TNode, TEdge, TUnit> : 
        Effect<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public ArmoredEffect(TUnit targetUnit) : base(targetUnit)
        {
        }

        public override EffectType EffectType => EffectType.Armored;

        public override void ExecuteOnEnter()
        {
            TargetUnit.UnitReplenished += OnReplenish;
        }

        public override void ExecuteOnExit()
        {
            TargetUnit.UnitReplenished -= OnReplenish;
        }

        private void OnReplenish(TUnit unit)
        {
            if (unit.CurrentArmor < TargetUnit.CurrentPower)
                TargetUnit.CurrentArmor = TargetUnit.CurrentPower;
        }
    }
}
