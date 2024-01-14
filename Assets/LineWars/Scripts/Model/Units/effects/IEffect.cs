namespace LineWars.Model
{
    public abstract class Effect<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public TUnit TargetUnit { get; private set; }
        public abstract EffectType EffectType { get; }
        public Effect(TUnit targetUnit)
        {
            TargetUnit = targetUnit;
        }

        public abstract void ExecuteOnEnter();
        public abstract void ExecuteOnExit();
    }
}
