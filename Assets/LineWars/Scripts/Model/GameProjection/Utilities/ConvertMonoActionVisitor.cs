using System;

namespace LineWars.Model
{
    public class ConvertMonoActionVisitor : IMonoUnitActionVisitor
    {
        public UnitProjection Unit { get; private set; }
        public IGraphForGame<NodeProjection, EdgeProjection, UnitProjection> Graph { get; private set; }
        public UnitAction<NodeProjection, EdgeProjection, UnitProjection> Result { get; private set; }

        public ConvertMonoActionVisitor(UnitProjection unit, IGraphForGame<NodeProjection, EdgeProjection, UnitProjection> graph)
        {
            Unit = unit;
            Graph = graph;
        }

        public void Visit(MonoBuildRoadAction action)
        {
            Result = new BuildAction<NodeProjection, EdgeProjection, UnitProjection>
                (Unit);
            Result.ActionModifier = action.Action.ActionModifier;
        }

        public void Visit(MonoBlockAction action)
        {
            Result = new BlockAction<NodeProjection, EdgeProjection, UnitProjection>
                (Unit, action.InitialContrAttackDamageModifier, action.Protection);
            Result.ActionModifier = action.Action.ActionModifier;
        }

        public void Visit(MonoMoveAction action)
        {
            Result = new MoveAction<NodeProjection, EdgeProjection, UnitProjection>(Unit);
            Result.ActionModifier = action.Action.ActionModifier;
        }

        public void Visit(MonoHealAction action)
        {
            Result = new HealAction<NodeProjection, EdgeProjection, UnitProjection>
                (Unit, action.IsMassHeal, action.HealingAmount);
            Result.ActionModifier = action.Action.ActionModifier;
        }

        public void Visit(MonoDistanceAttackAction action)
        {
            Result = new DistanceAttackAction<NodeProjection, EdgeProjection, UnitProjection>
                (Unit, action.Damage, action.IsPenetratingDamage, action.Distance, Graph);
            Result.ActionModifier = action.Action.ActionModifier;
        }

        public void Visit(MonoArtilleryAttackAction action)
        {
            Result = new ArtilleryAttackAction<NodeProjection, EdgeProjection, UnitProjection>
                (Unit, action.Damage, action.IsPenetratingDamage, action.Distance, Graph);
            Result.ActionModifier = action.Action.ActionModifier;
        }

        public void Visit(MonoMeleeAttackAction action)
        {
            Result = new MeleeAttackAction<NodeProjection, EdgeProjection, UnitProjection>
                (Unit, action.Damage, action.IsPenetratingDamage, action.Onslaught, action.BlockerSelector);
            Result.ActionModifier = action.Action.ActionModifier;
        }

        public void Visit(MonoRLBlockAction action)
        {
            Result = new RLBlockAction<NodeProjection, EdgeProjection, UnitProjection>(Unit);
            Result.ActionModifier = action.Action.ActionModifier;
        }

        public void Visit(MonoSacrificeForPerunAction action)
        {
            Result = new SacrificeForPerunAction<NodeProjection, EdgeProjection, UnitProjection>(Unit);
            Result.ActionModifier = action.Action.ActionModifier;
        }

        public void Visit(MonoRamAction action)
        {
            Result = new RamAction<NodeProjection, EdgeProjection, UnitProjection>(Unit, action.Damage);
            Result.ActionModifier = action.Action.ActionModifier;
        }

        public void Visit(MonoBlowWithSwingAction action)
        {
            Result = new BlowWithSwingAction<NodeProjection, EdgeProjection, UnitProjection>(Unit, action.Damage);
            Result.ActionModifier = action.Action.ActionModifier;
        }

        public void Visit(MonoShotUnitAction action)
        {
            Result = new ShotUnitAction<NodeProjection, EdgeProjection, UnitProjection>(Unit);
            Result.ActionModifier = action.Action.ActionModifier;
        }

        public void Visit(MonoRLBuildAction action)
        {
            Result = new RLBuildAction<NodeProjection, EdgeProjection, UnitProjection>
                (Unit, action.PossibleBuildings, action.Factory);
            Result.ActionModifier = action.Action.ActionModifier;
        }

        public static ConvertMonoActionVisitor Create(UnitProjection unit, 
            IGraphForGame<NodeProjection, EdgeProjection, UnitProjection> graph)

        {
            return new ConvertMonoActionVisitor(unit, graph);
        }
    }

    
}
