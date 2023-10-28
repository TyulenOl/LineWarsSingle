using System;

namespace LineWars.Model
{
    public class ConvertMonoActionVisitor : IMonoUnitVisitor
    {
        public UnitProjection Unit { get; private set; }
        public IGraphForGame<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection, BasePlayerProjection> Graph { get; private set; }
        public UnitAction<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection, BasePlayerProjection> Result { get; private set; }

        public ConvertMonoActionVisitor(UnitProjection unit, IGraphForGame<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection, BasePlayerProjection> graph)
        {
            Unit = unit;
            Graph = graph;
        }

        public void Visit(MonoBuildRoadAction action)
        {
            Result = new BuildAction<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection, BasePlayerProjection>(Unit);
        }

        public void Visit(MonoBlockAction action)
        {
            Result = new BlockAction<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection, BasePlayerProjection>(Unit, action.InitialContrAttackDamageModifier, action.Protection);
        }

        public void Visit(MonoMoveAction action)
        {
            Result = new MoveAction<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection, BasePlayerProjection>(Unit);
        }

        public void Visit(MonoHealAction action)
        {
            Result = new HealAction<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection, BasePlayerProjection>(Unit, action.IsMassHeal, action.HealingAmount);
        }

        public void Visit(MonoDistanceAttackAction action)
        {
            Result = new DistanceAttackAction<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection, BasePlayerProjection>(Unit, action.Damage, action.IsPenetratingDamage, action.Distance, Graph);
        }

        public void Visit(MonoArtilleryAttackAction action)
        {
            Result = new ArtilleryAttackAction<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection, BasePlayerProjection>(Unit, action.Damage, action.IsPenetratingDamage, action.Distance, Graph);
        }

        public void Visit(MonoMeleeAttackAction action)
        {
            Result = new MeleeAttackAction<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection, BasePlayerProjection>(Unit, action.Damage, action.IsPenetratingDamage, action.Onslaught, action.BlockerSelector);
        }

        public void Visit(MonoRLBlockAction action)
        {
            Result = new RLBlockAction<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection, BasePlayerProjection>(Unit);
        }

        public void Visit(MonoSacrificeForPerunAction action)
        {
            Result = new SacrificeForPerunAction<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection, BasePlayerProjection>(Unit);
        }

        public void Visit(MonoRamAction action)
        {
            Result = new RamAction<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection, BasePlayerProjection>(Unit, action.Damage);
        }

        public void Visit(MonoBlowWithSwingAction action)
        {
            Result = new BlowWithSwingAction<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection, BasePlayerProjection>(Unit, action.Damage);
        }

        public void Visit(MonoShotUnitAction action)
        {
            Result = new ShotUnitAction<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection, BasePlayerProjection>(Unit);
        }

        public void Visit(MonoRLBuildAction action)
        {
            Result = new RLBuildAction<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection, BasePlayerProjection>(Unit, action.PossibleBuildings, action.Factory);
        }

        public static ConvertMonoActionVisitor Create(UnitProjection unit, IGraphForGame<NodeProjection, EdgeProjection, UnitProjection, OwnedProjection, BasePlayerProjection> graph)

        {
            return new ConvertMonoActionVisitor(unit, graph);
        }
    }

    
}
