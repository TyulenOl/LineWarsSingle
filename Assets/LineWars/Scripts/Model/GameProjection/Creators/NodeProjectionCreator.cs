using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public static class NodeProjectionCreator
    { 
        public static NodeProjection FromMono(
            Node original,
            IEnumerable<EdgeProjection> edgeProjections = null,
            UnitProjection leftUnit = null,
            UnitProjection rightUnit = null)
        {
            var newNode = new NodeProjection
            {
                CommandPriorityData = original.CommandPriorityData,
                IsBase = original.IsBase,
                Id = original.Id,
                Visibility = original.Visibility,
                ValueOfHidden = original.ValueOfHidden,
                LeftUnit = leftUnit,
                RightUnit = rightUnit,
                Original = original,
                EdgesList = edgeProjections == null
                    ? new List<EdgeProjection>() 
                    : new List<EdgeProjection>(edgeProjections)
            };

            if (original.TryGetComponent(out NodeScore nodeScore))
                newNode.Score = nodeScore.Score;
            else
                newNode.Score = 1;

            return newNode;
        }

        public static NodeProjection FromProjection(
            IReadOnlyNodeProjection oldNode,
            IEnumerable<EdgeProjection> edges = null,
            UnitProjection leftUnit = null,
            UnitProjection rightUnit = null)
        {
            var newNode = new NodeProjection
            {
                CommandPriorityData = oldNode.CommandPriorityData,
                IsBase = oldNode.IsBase,
                Id = oldNode.Id,
                Visibility = oldNode.Visibility,
                ValueOfHidden = oldNode.ValueOfHidden,
                LeftUnit = leftUnit,
                RightUnit = rightUnit,
                Original = oldNode.Original,
                EdgesList = edges == null ?
                    new List<EdgeProjection>() : new List<EdgeProjection>(edges),
                Score = oldNode.Score
            };

            return newNode;

        }
    }
}
