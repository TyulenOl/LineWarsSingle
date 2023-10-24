using System;

namespace LineWars.Model
{
    public class BlockAttackCommandBlueprint : ICommandBlueprint
    {
        public enum Target
        {
            Edge,
            Unit
        }
        public Target TargetType { get; private set; }
        public int UnitId { get; private set; }
        public int TargetId { get; private set; }

        public BlockAttackCommandBlueprint(int unitId, int targetId, Target targetType)
        {
            UnitId = unitId;
            TargetId = targetId;
            TargetType = targetType;
        }

        public ICommand GenerateCommand(GameProjection projection)
        {
            var unit = projection.UnitsIndexList[UnitId];
            switch (TargetType)
            {
                case Target.Edge:
                    var edge = projection.EdgesIndexList[TargetId];
                    return new BlockAttackCommand
                        <NodeProjection, EdgeProjection, UnitProjection,
                        OwnedProjection, BasePlayerProjection>(unit, edge);
                case Target.Unit:
                    var targetUnit = projection.UnitsIndexList[TargetId];
                    return new BlockAttackCommand
                        <NodeProjection, EdgeProjection, UnitProjection,
                        OwnedProjection, BasePlayerProjection>(unit, targetUnit);
            }

            throw new ArgumentException("Faild to generate Command!");
        }

        public ICommand GenerateMonoCommand(GameProjection projection)
        {
            var unit = projection.UnitsIndexList[UnitId].Original;
            switch (TargetType)
            {
                case Target.Edge:
                    var edge = projection.EdgesIndexList[TargetId].Original;
                    return new BlockAttackCommand
                        <Node, Edge, Unit,
                        Owned, BasePlayer>(unit, edge);
                case Target.Unit:
                    var targetUnit = projection.UnitsIndexList[TargetId].Original;
                    return new BlockAttackCommand
                        <Node, Edge, Unit,
                        Owned, BasePlayer>(unit, targetUnit);
            }

            throw new ArgumentException("Faild to generate Command!");
        }
    }
}
