using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class HealCommandBlueprint : ICommandBlueprint
    {
        public int ExecutorId { get; private set; }
        public int UnitId { get; private set; }

        public HealCommandBlueprint(int doctorId, int unitId)
        {
            ExecutorId = doctorId;
            UnitId = unitId;
        }
    
        public ICommand GenerateCommand(GameProjection projection)
        {
            var doctor = projection.UnitsIndexList[UnitId];
            var unit = projection.UnitsIndexList[UnitId];

            return new HealCommand<NodeProjection, EdgeProjection, UnitProjection>(doctor, unit);
        }

        public ICommand GenerateMonoCommand(GameProjection projection)
        {
            var doctor = projection.UnitsIndexList[UnitId].Original;
            var unit = projection.UnitsIndexList[UnitId].Original;

            return new HealCommand<Node, Edge, Unit>(doctor, unit);
        }
    }
}
