using System;

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
            throw new NotImplementedException();
        }

        public void Visit(MonoBlockAction action)
        {
            throw new NotImplementedException();
        }

        public void Visit(MonoMoveAction action)
        {
            throw new NotImplementedException();
        }

        public void Visit(MonoHealAction action)
        {
            throw new NotImplementedException();
        }

        public void Visit(MonoDistanceAttackAction action)
        {
            throw new NotImplementedException();
        }

        public void Visit(MonoArtilleryAttackAction action)
        {
            throw new NotImplementedException();
        }

        public void Visit(MonoMeleeAttackAction action)
        {
            throw new NotImplementedException();
        }

        public void Visit(MonoRLBlockAction action)
        {
            throw new NotImplementedException();
        }

        public void Visit(MonoSacrificeForPerunAction action)
        {
            throw new NotImplementedException();
        }

        public void Visit(MonoRamAction action)
        {
            throw new NotImplementedException();
        }

        public void Visit(MonoBlowWithSwingAction action)
        {
            throw new NotImplementedException();
        }

        public void Visit(MonoShotUnitAction action)
        {
            throw new NotImplementedException();
        }

        public void Visit(MonoRLBuildAction action)
        {
            throw new NotImplementedException();
        }
    }

    public static class ConvertMonoActionVisitor
    {
        public static ConvertMonoActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> Create<TNode, TEdge, TUnit, TOwned, TPlayer>(TUnit unit, IGraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer> graph)
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
