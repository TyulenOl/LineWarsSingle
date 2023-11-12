namespace LineWars.Model
{
    public interface IBaseUnitActionVisitor<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public void Visit(BuildAction<TNode, TEdge, TUnit> action);
        public void Visit(BlockAction<TNode, TEdge, TUnit> action);
        public void Visit(MoveAction<TNode, TEdge, TUnit> action);
        public void Visit(HealAction<TNode, TEdge, TUnit> action);
        public void Visit(DistanceAttackAction<TNode, TEdge, TUnit> action);
        public void Visit(ArtilleryAttackAction<TNode, TEdge, TUnit> action);
        public void Visit(MeleeAttackAction<TNode, TEdge, TUnit> action);
        public void Visit(RLBlockAction<TNode, TEdge, TUnit> action);
        public void Visit(SacrificeForPerunAction<TNode, TEdge, TUnit> action);
        public void Visit(RamAction<TNode, TEdge, TUnit> action);
        public void Visit(BlowWithSwingAction<TNode, TEdge, TUnit> action);
        public void Visit(ShotUnitAction<TNode, TEdge, TUnit> action);
        public void Visit(RLBuildAction<TNode, TEdge, TUnit> action);
    }
}