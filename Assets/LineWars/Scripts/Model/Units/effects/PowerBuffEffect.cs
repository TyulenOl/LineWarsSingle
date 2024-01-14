namespace LineWars.Model
{
    public class PowerBuffEffect<TNode, TEdge, TUnit> : Effect<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private readonly int addedPower;
        public PowerBuffEffect(TUnit unit, int addedPower) : base(unit)
        {
            this.addedPower = addedPower;
        }

        public override EffectType EffectType => EffectType.PowerBuff;

        public override void ExecuteOnEnter()
        {
            UnitOwner.CurrentPower += addedPower;
        }

        public override void ExecuteOnExit()
        {
            UnitOwner.CurrentPower -= addedPower;
        }

        public override void ExecuteOnReplenish()
        {
            
        }
    }
}
