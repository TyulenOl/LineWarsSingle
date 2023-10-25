namespace LineWars.Model
{
    public class ConvertMonoActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> : IMonoUnitVisitor
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion
    {
        public TUnit Unit { get; private set; }
        public IGraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer> Graph { get; private set; }
        public UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer> Result { get; private set; }

        public ConvertMonoActionVisitor(TUnit unit, IGraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer> graph)
        {
            Unit = unit;
            Graph = graph;
        }

        public void Visit(MonoBuildRoadAction action)
        {
            Result = new BuildAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action);
        }

        public void Visit(MonoBlockAction action)
        {
            Result = new BlockAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action);
        }

        public void Visit(MonoMoveAction action)
        {
            Result = new MoveAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action);
        }

        public void Visit(MonoHealAction action)
        {
            Result = new HealAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action);
        }

        public void Visit(MonoDistanceAttackAction action)
        {
            Result = new DistanceAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action, Graph);
        }

        public void Visit(MonoArtilleryAttackAction action)
        {
            Result = new ArtilleryAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action, Graph);
        }

        public void Visit(MonoMeleeAttackAction action)
        {
            Result = new MeleeAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action);
        }

        public void Visit(MonoRLBlockAction action)
        {
            Result = new RLBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action);
        }

        public void Visit(MonoSacrificeForPerunAction action)
        {
            Result = new SacrificeForPerunAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action);
        }

        public void Visit(MonoRamAction action)
        {
            Result = new RamAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action);
        }

        public void Visit(MonoBlowWithSwingAction action)
        {
            Result = new BlowWithSwingAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action);
        }

        public void Visit(MonoShotUnitAction action)
        {
            Result = new ShotUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action);
        }
    }

    public static class ConvertMonoActionVisitor
    {
        public static ConvertMonoActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer>
            Create<TNode, TEdge, TUnit, TOwned, TPlayer>(TUnit unit, IGraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer> graph)
            #region Сonstraints
            where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
            where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
            where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
            where TOwned : class, IOwned<TOwned, TPlayer>
            where TPlayer : class, IBasePlayer<TOwned, TPlayer>
            #endregion
        {
            return new ConvertMonoActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> (unit, graph);
        }
    }
}
