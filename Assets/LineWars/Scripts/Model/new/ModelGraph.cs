using System.Collections.Generic;

namespace LineWars.Model
{
    public class ModelGraph
    {
        private readonly List<ModelNode> nodes;
        private readonly List<ModelEdge> edges;

        public IReadOnlyCollection<ModelNode> Nodes => nodes;
        public IReadOnlyCollection<ModelEdge> Edges => edges;

        public ModelGraph(List<ModelNode> nodes, List<ModelEdge> edges)
        {
            this.nodes = nodes;
            this.edges = edges;
        }
    }
}