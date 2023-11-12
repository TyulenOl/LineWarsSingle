using System;

namespace LineWars.Model
{
    public class CopyActionVisitor<TNode, TEdge, TUnit> : 
        IBaseUnitActionVisitor<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit> 
    {
        public TUnit Unit { get; private set; }
        public IGraphForGame<TNode, TEdge, TUnit> Graph { get; private set; }
        public UnitAction<TNode, TEdge, TUnit> Result { get; private set; }

        public CopyActionVisitor(TUnit unit, IGraphForGame<TNode, TEdge, TUnit> graph)
        {
            Unit = unit;
            Graph = graph;
        }

        public void Visit(BuildAction<TNode, TEdge, TUnit> action)
        {
            Result = new BuildAction<TNode, TEdge, TUnit>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(BlockAction<TNode, TEdge, TUnit> action)
        {
            Result = new BlockAction<TNode, TEdge, TUnit>(Unit, action.ContrAttackDamageModifier, action.Protection);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(MoveAction<TNode, TEdge, TUnit> action)
        {
            Result = new MoveAction<TNode, TEdge, TUnit>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(HealAction<TNode, TEdge, TUnit> action)
        {
            Result = new HealAction<TNode, TEdge, TUnit>(Unit, action.IsMassHeal, action.HealingAmount);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(DistanceAttackAction<TNode, TEdge, TUnit> action)
        {
            Result = new DistanceAttackAction
                <TNode, TEdge, TUnit>(Unit, action.Damage, action.IsPenetratingDamage, action.Distance, Graph);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(ArtilleryAttackAction<TNode, TEdge, TUnit> action)
        {
            Result = new ArtilleryAttackAction<TNode, TEdge, TUnit>(Unit, action.Damage, action.IsPenetratingDamage, action.Distance, Graph);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(MeleeAttackAction<TNode, TEdge, TUnit> action)
        {
            Result = new MeleeAttackAction<TNode, TEdge, TUnit>(Unit, action.Damage, action.IsPenetratingDamage, action.Onslaught, action.BlockerSelector);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(RLBlockAction<TNode, TEdge, TUnit> action)
        {
            Result = new RLBlockAction<TNode, TEdge, TUnit>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(SacrificeForPerunAction<TNode, TEdge, TUnit> action)
        {
            Result = new SacrificeForPerunAction<TNode, TEdge, TUnit>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(RamAction<TNode, TEdge, TUnit> action)
        {
            Result = new RamAction<TNode, TEdge, TUnit>(Unit, action.Damage);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(BlowWithSwingAction<TNode, TEdge, TUnit> action)
        {
            Result = new BlowWithSwingAction<TNode, TEdge, TUnit>(Unit, action.Damage);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(ShotUnitAction<TNode, TEdge, TUnit> action)
        {
            Result = new ShotUnitAction<TNode, TEdge, TUnit>(Unit);
            Result.ActionModifier = action.ActionModifier;
        }

        public void Visit(RLBuildAction<TNode, TEdge, TUnit> action)
        {
            Result = new RLBuildAction<TNode, TEdge, TUnit>(Unit, action.PossibleBuildings, action.Factory);
            Result.ActionModifier = action.ActionModifier;
        }
    }

    public static class CopyActionVisitor
    {
        public static CopyActionVisitor<TNode, TEdge, TUnit> 
            Create<TNode, TEdge, TUnit>(TUnit unit, IGraphForGame<TNode, TEdge, TUnit> graph)
            where TNode : class, INodeForGame<TNode, TEdge, TUnit>
            where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
            where TUnit : class, IUnit<TNode, TEdge, TUnit>

        {
            return new CopyActionVisitor<TNode, TEdge, TUnit> (unit, graph);
        }
    }
}
