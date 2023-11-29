namespace LineWars.Model
{
    public static class EdgeProjectionCreator
    {
        public static EdgeProjection FromMono(
            Edge original,
            NodeProjection firstNode = null,
            NodeProjection secondNode = null)
        {
            var newEdge = new EdgeProjection
            {
                CommandPriorityData = original.CommandPriorityData,
                Id = original.Id,
                MaxHp = original.MaxHp,
                CurrentHp = original.CurrentHp,
                LineType = original.LineType,
                FirstNode = firstNode,
                SecondNode = secondNode,
                Original = original
            };

            return newEdge;
        }

        public static EdgeProjection FromProjection(
            IReadOnlyEdgeProjection edge,
            NodeProjection firstNode = null,
            NodeProjection secondNode = null)
        {
            var newEdge = new EdgeProjection
            {
                CommandPriorityData = edge.CommandPriorityData,
                Id = edge.Id,
                MaxHp = edge.MaxHp,
                CurrentHp = edge.CurrentHp,
                LineType = edge.LineType,
                Original = edge.Original,
                FirstNode = firstNode,
                SecondNode = secondNode
            };

            return newEdge;
        }
    }
}
