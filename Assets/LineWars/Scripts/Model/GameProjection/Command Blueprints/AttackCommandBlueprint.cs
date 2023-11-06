using System;

namespace LineWars.Model
{
    public class AttackCommandBlueprint : ICommandBlueprint
    {
        public enum Target
        {
            Edge,
            Unit
        }
        public Target TargetType { get; private set; }
        public int ExecutorId { get; private set; }
        public int TargetId { get; private set; }

        public AttackCommandBlueprint(int unitId, int targetId, Target targetType)
        {
            ExecutorId = unitId;
            TargetId = targetId;
            TargetType = targetType;
        }

        public ICommand GenerateCommand(GameProjection projection)
        {
            var unit = projection.UnitsIndexList[ExecutorId];
            switch(TargetType)
            {
                case Target.Edge:
                    var edge = projection.EdgesIndexList[TargetId];
                    return new AttackCommand<NodeProjection, EdgeProjection, UnitProjection>(unit, edge);
                case Target.Unit:
                    var targetUnit = projection.UnitsIndexList[TargetId];
                    return new AttackCommand<NodeProjection, EdgeProjection, UnitProjection>(unit, targetUnit);
            }

            throw new ArgumentException("Failed to generate Command!");
        }

        public ICommand GenerateMonoCommand(GameProjection projection)
        {
            var unit = projection.UnitsIndexList[ExecutorId].Original;
            switch (TargetType)
            {
                case Target.Edge:
                    var edge = projection.EdgesIndexList[TargetId].Original;
                    return new AttackCommand<Node, Edge, Unit>(unit, edge);
                case Target.Unit:
                    var targetUnit = projection.UnitsIndexList[TargetId].Original;
                    return new AttackCommand<Node, Edge, Unit>(unit, targetUnit);
            }

            throw new ArgumentException("Failed to generate Command!");
        }
    }
}
