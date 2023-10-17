using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class HealCommandBlueprint : ICommandBlueprint
    {
        public int DoctorId { get; private set; }
        public int UnitId { get; private set; }

        public HealCommandBlueprint(int doctorId, int unitId)
        {
            DoctorId = doctorId;
            UnitId = unitId;
        }
    
        public ICommand GenerateCommand(GameProjection projection)
        {
            var doctor = projection.UnitsIndexList[UnitId];
            var unit = projection.UnitsIndexList[UnitId];

            return new HealCommand<NodeProjection, EdgeProjection, UnitProjection, 
                OwnedProjection, BasePlayerProjection>(doctor, unit);
        }

        public ICommand GenerateMonoCommand(GameProjection projection)
        {
            var doctor = projection.UnitsIndexList[UnitId].Original;
            var unit = projection.UnitsIndexList[UnitId].Original;

            return new HealCommand<Node, Edge, Unit,
                Owned, BasePlayer>(doctor, unit);
        }
    }
}
