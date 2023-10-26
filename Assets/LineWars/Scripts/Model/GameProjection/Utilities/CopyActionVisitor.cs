using System;

namespace LineWars.Model
{
    public class CopyActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> : IUnitActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer>
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

        public CopyActionVisitor(TUnit unit, IGraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer> graph)
        {
            Unit = unit;
            Graph = graph;
        }

        public void Visit(BuildAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            throw new NotImplementedException();
            //Result = new BuildAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action);
        }

        public void Visit(BlockAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            throw new NotImplementedException();
            //Result = new BlockAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action);
        }

        public void Visit(MoveAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            throw new NotImplementedException();
            //Result = new MoveAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action);
        }

        public void Visit(HealAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            throw new NotImplementedException();
            //Result = new HealAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action);
        }

        public void Visit(DistanceAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            throw new NotImplementedException();
            //Result = new DistanceAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action, Graph);
        }

        public void Visit(ArtilleryAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            throw new NotImplementedException();
            //Result = new ArtilleryAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action, Graph);
        }

        public void Visit(MeleeAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            throw new NotImplementedException();
            //Result = new MeleeAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action);
        }

        public void Visit(RLBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            throw new NotImplementedException();
            //Result = new RLBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action);
        }

        public void Visit(SacrificeForPerunAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            throw new NotImplementedException();
            //Result = new SacrificeForPerunAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action);
        }

        public void Visit(RamAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            throw new NotImplementedException();
           //Result = new RamAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action);
        }

        public void Visit(BlowWithSwingAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            throw new NotImplementedException();
            //Result = new BlowWithSwingAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action);
        }

        public void Visit(ShotUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            throw new NotImplementedException();
            //Result = new ShotUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action);
        }

        public void Visit(RLBuilderAction<TNode, TEdge, TUnit, TOwned, TPlayer> ramAction)
        {
            throw new NotImplementedException();
        }
    }

    public static class CopyActionVisitor
    {
        public static CopyActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> 
            Create<TNode, TEdge, TUnit, TOwned, TPlayer>(TUnit unit, IGraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer> graph)
            #region Сonstraints
            where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
            where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
            where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
            where TOwned : class, IOwned<TOwned, TPlayer>
            where TPlayer : class, IBasePlayer<TOwned, TPlayer>
            #endregion

        {
            return new CopyActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> (unit, graph);
        }
    }
}
