using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public static class NodeProjectionCreator
    { 
        public static NodeProjection FromMono(Node original,
            IEnumerable<EdgeProjection> edgeProjections = null, UnitProjection leftUnit = null,
            UnitProjection rightUnit = null)
        {
            var newNode = new NodeProjection();
            newNode.CommandPriorityData = original.CommandPriorityData;
            newNode.IsBase = original.IsBase;
            newNode.Id = original.Id;
            newNode.Visibility = original.Visibility;
            newNode.ValueOfHidden = original.ValueOfHidden;
            newNode.LeftUnit = leftUnit;
            newNode.RightUnit = rightUnit;
            newNode.Original = original;
            newNode.EdgesList = edgeProjections == null ?
                new List<EdgeProjection>() : new List<EdgeProjection>(edgeProjections);

            if (original.TryGetComponent(out NodeScore nodeScore))
                newNode.Score = nodeScore.Score;
            else
                newNode.Score = 1;

            return newNode;
        }

        public static NodeProjection FromProjection(IReadOnlyNodeProjection oldNode, IEnumerable<EdgeProjection> edges = null,
            UnitProjection leftUnit = null, UnitProjection rightUnit = null)
        {
            var newNode = new NodeProjection();
            newNode.CommandPriorityData = oldNode.CommandPriorityData;
            newNode.IsBase = oldNode.IsBase;
            newNode.Id = oldNode.Id;
            newNode.Visibility = oldNode.Visibility;
            newNode.ValueOfHidden = oldNode.ValueOfHidden;
            newNode.LeftUnit = leftUnit;
            newNode.RightUnit = rightUnit;
            newNode.Original = oldNode.Original;
            newNode.EdgesList = edges == null ?
                new List<EdgeProjection>() : new List<EdgeProjection>(edges);
            newNode.Score = oldNode.Score;

            return newNode;

        }
    }
}
