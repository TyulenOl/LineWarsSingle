using System;
using System.Linq;
using JetBrains.Annotations;

namespace LineWars.Model
{
    public class GameStateHelper
    {
        public float GetGlobalPercentOfMapCapture() => GetPercentOfMapCapture(_ => true);

        public float GetPercentOfMapCaptureInVisibilityAreaFor(BasePlayer basePlayer)
        {
            var visibilityMap = Graph.GetVisibilityInfo(basePlayer);
            return GetPercentOfMapCapture(x => visibilityMap[x]);
        }
        public float GetPercentOfMapCapture([NotNull] Func<Node, bool> nodeSelectCondition)
        {
            if (nodeSelectCondition == null) throw new ArgumentNullException(nameof(nodeSelectCondition));

            float allNodesCount = Graph.AllNodes.Count;
            float captureNodesCount = Graph.AllNodes
                .Where(nodeSelectCondition)
                .Count(x => x.Owner != null);
            return captureNodesCount / allNodesCount;
        }
    }
}