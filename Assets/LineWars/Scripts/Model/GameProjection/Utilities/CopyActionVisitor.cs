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
            Result = new BuildAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(BlockAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            Result = new BlockAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action.ContrAttackDamageModifier, action.Protection);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(MoveAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            Result = new MoveAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(HealAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            Result = new HealAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action.IsMassHeal, action.HealingAmount);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(DistanceAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            Result = new DistanceAttackAction
                <TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action.Damage, action.IsPenetratingDamage, action.Distance, Graph);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(ArtilleryAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            Result = new ArtilleryAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action.Damage, action.IsPenetratingDamage, action.Distance, Graph);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(MeleeAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            Result = new MeleeAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action.Damage, action.IsPenetratingDamage, action.Onslaught, action.BlockerSelector);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(RLBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            Result = new RLBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(SacrificeForPerunAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            Result = new SacrificeForPerunAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(RamAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            Result = new RamAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action.Damage);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(BlowWithSwingAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            Result = new BlowWithSwingAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action.Damage);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(ShotUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            Result = new ShotUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(RLBuildAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            Result = new RLBuildAction<TNode, TEdge, TUnit, TOwned, TPlayer>(Unit, action.PossibleBuildings, action.Factory);
            Result.ActionModifier = action.ActionModifier;
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
