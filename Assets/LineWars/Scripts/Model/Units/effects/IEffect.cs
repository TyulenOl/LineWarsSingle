namespace LineWars.Model
{
    public abstract class Effect<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public TUnit UnitOwner { get; private set; }
        public EffectType EffectType { get; }
        public Effect(TUnit unit)
        {
            UnitOwner = unit;
        }

        public abstract void ExecuteOnEnter();
        public abstract void ExecuteOnExit();
        public abstract void ExecuteOnReplenish();
    }
}
