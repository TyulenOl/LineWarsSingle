using System;

namespace LineWars.Model
{
    public class ContrAttackCommandBlueprint : ICommandBlueprint
    {
        public int ExecutorId { get; private set; }
        public int TargetId { get; private set; }

        public ContrAttackCommandBlueprint(int unitId, int targetId)
        {
            ExecutorId = unitId;
            TargetId = targetId;
        }

        public ICommand GenerateCommand(GameProjection projection)
        {
            var unit = projection.UnitsIndexList[ExecutorId];
            var targetUnit = projection.UnitsIndexList[TargetId];
            return new ContrAttackCommand
                <NodeProjection, EdgeProjection, UnitProjection>(unit, targetUnit);
        }

        public ICommand GenerateMonoCommand(GameProjection projection)
        {
            var unit = projection.UnitsIndexList[ExecutorId].Original;
            var targetUnit = projection.UnitsIndexList[TargetId].Original;
            return new ContrAttackCommand<Node, Edge, Unit>(unit, targetUnit);
        }
    }
}
