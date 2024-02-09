namespace LineWars.Model
{
    public class PowerBuffEffect<TNode, TEdge, TUnit> : 
        Effect<TNode, TEdge, TUnit>, 
        IPowerEffect
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private readonly int addedPower;
        public PowerBuffEffect(TUnit targetUnit, int addedPower) : base(targetUnit)
        {
            this.addedPower = addedPower;
        }

        public override EffectType EffectType => EffectType.PowerBuff;

        public int Power => addedPower;

        public override void ExecuteOnEnter()
        {
            TargetUnit.CurrentPower += addedPower;
        }

        public override void ExecuteOnExit()
        {
            TargetUnit.CurrentPower -= addedPower;
        }
    }
}
