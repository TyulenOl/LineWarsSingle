using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class SwingCommandBlueprint : ICommandBlueprint
    {
        public int ExecutorId { get; private set; }
        public int targetUnitId { get; private set; }

        public SwingCommandBlueprint(int executorId, int targetUnitId)
        {
            ExecutorId = executorId;
            this.targetUnitId = targetUnitId;    
        }

        public ICommand GenerateCommand(GameProjection projection)
        {
            var unit = projection.UnitsIndexList[ExecutorId];
            var target = projection.UnitsIndexList[targetUnitId];
            return new BlowWithSwingCommand<NodeProjection, EdgeProjection, UnitProjection>(unit, target);
        }

        public ICommand GenerateMonoCommand(GameProjection projection)
        {
            var unit = projection.UnitsIndexList[ExecutorId].Original;
            var target = projection.UnitsIndexList[targetUnitId].Original;
            return new BlowWithSwingCommand<Node, Edge, Unit>(unit, target);
        }
    }
}
