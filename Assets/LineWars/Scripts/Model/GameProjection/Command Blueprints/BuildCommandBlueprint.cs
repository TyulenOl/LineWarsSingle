using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

namespace LineWars.Model
{
    public class BuildCommandBlueprint : ICommandBlueprint
    {
        public int ExecutorId { get; private set; }
        public int EdgeId { get; private set; }

        public BuildCommandBlueprint(int engineerId, int edgeId)
        {
            ExecutorId = engineerId;
            EdgeId = edgeId;
        }

        public ICommand GenerateCommand(GameProjection projection)
        {
            var engineer = projection.UnitsIndexList[ExecutorId];
            var edge = projection.EdgesIndexList[EdgeId];   

            return new BuildCommand
                <NodeProjection, EdgeProjection, UnitProjection>(engineer, edge);
        }

        public ICommand GenerateMonoCommand(GameProjection projection)
        {
            var engineer = projection.UnitsIndexList[ExecutorId].Original;
            var edge = projection.EdgesIndexList[EdgeId].Original;

            return new BuildCommand
                <Node, Edge, Unit>(engineer, edge);
        }
    }
}
