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
            var visibilityMap = MonoGraph.Instance.GetVisibilityInfo(basePlayer);
            return GetPercentOfMapCapture(x => visibilityMap[x]);
        }
        public float GetPercentOfMapCapture([NotNull] Func<Node, bool> nodeSelectCondition)
        {
            if (nodeSelectCondition == null) throw new ArgumentNullException(nameof(nodeSelectCondition));

            float allNodesCount = MonoGraph.Instance.Nodes.Count();
            float captureNodesCount = MonoGraph.Instance.Nodes
                .Where(nodeSelectCondition)
                .Count(x => x.Owner != null);
            return captureNodesCount / allNodesCount;
        }
    }
}