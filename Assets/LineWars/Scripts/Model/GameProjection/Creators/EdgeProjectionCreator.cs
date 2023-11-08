namespace LineWars.Model
{
    public static class EdgeProjectionCreator
    {
        public static EdgeProjection FromMono(Edge original,
            NodeProjection firstNode = null, NodeProjection secondNode = null)
        {
            var newEdge = new EdgeProjection();
            newEdge.CommandPriorityData = original.CommandPriorityData;
            newEdge.Id = original.Id;
            newEdge.MaxHp = original.MaxHp;
            newEdge.CurrentHp = original.CurrentHp;
            newEdge.LineType = original.LineType;
            newEdge.FirstNode = firstNode;
            newEdge.SecondNode = secondNode;
            newEdge.Original = original;

            return newEdge;
        }

        public static EdgeProjection FromProjection(IReadOnlyEdgeProjection edge,
            NodeProjection firstNode = null, NodeProjection secondNode = null)
        {
            var newEdge = new EdgeProjection();
            newEdge.CommandPriorityData = edge.CommandPriorityData;
            newEdge.Id = edge.Id;
            newEdge.MaxHp = edge.MaxHp;
            newEdge.CurrentHp = edge.CurrentHp;
            newEdge.LineType = edge.LineType;
            newEdge.Original = edge.Original;
            newEdge.FirstNode = firstNode;
            newEdge.SecondNode = secondNode;

            return newEdge;
        }
    }
}
