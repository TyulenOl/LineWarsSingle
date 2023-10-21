using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

namespace LineWars.Model
{
    public class BuildCommandBlueprint : ICommandBlueprint
    {
        public int EngineerId { get; private set; }
        public int EdgeId { get; private set; }

        public BuildCommandBlueprint(int engineerId, int edgeId)
        {
            EngineerId = engineerId;
            EdgeId = edgeId;
        }

        public ICommand GenerateCommand(GameProjection projection)
        {
            var engineer = projection.UnitsIndexList[EngineerId];
            var edge = projection.EdgesIndexList[EdgeId];   

            return new BuildCommand
                <NodeProjection, EdgeProjection, UnitProjection, 
                OwnedProjection, BasePlayerProjection>
                (engineer, edge);
        }

        public ICommand GenerateMonoCommand(GameProjection projection)
        {
            var engineer = projection.UnitsIndexList[EngineerId].Original;
            var edge = projection.EdgesIndexList[EdgeId].Original;

            return new BuildCommand
                <Node, Edge, Unit, Owned, BasePlayer>(engineer, edge);
        }
    }
}
