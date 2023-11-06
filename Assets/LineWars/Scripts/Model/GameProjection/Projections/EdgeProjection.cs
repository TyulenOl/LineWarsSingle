namespace LineWars.Model
{
    public class EdgeProjection : 
        OwnedProjection, 
        IEdgeForGame<NodeProjection, EdgeProjection, UnitProjection>,
        IReadOnlyEdgeProjection
    {
        public Edge Original { get; private set; }
        public CommandPriorityData CommandPriorityData { get; private set; }
        public NodeProjection FirstNode { get; private set; }
        public NodeProjection SecondNode { get; private set; }
        public int Id { get; private set; }
        public int MaxHp { get; set; }
        public int CurrentHp { get; set; }
        public LineType LineType { get; set; }

        public bool HasOriginal => Original != null;

        public EdgeProjection(CommandPriorityData commandPriorityData,
            int index, int currentHp, LineType lineType, Edge original = null,
             NodeProjection firstNode = null, NodeProjection secondNode = null)
        {
            CommandPriorityData = commandPriorityData;
            FirstNode = firstNode;
            SecondNode = secondNode;
            Id = index;
            CurrentHp = currentHp;
            LineType = lineType;
            Original = original;
        }

        public EdgeProjection(IReadOnlyEdgeProjection edge, 
            NodeProjection firstNode = null, NodeProjection secondNode = null)
            : this(edge.CommandPriorityData,
                  edge.Id, edge.CurrentHp, edge.LineType, edge.Original,
                  firstNode, secondNode)
        { }

        public EdgeProjection(Edge original,
            NodeProjection firstNode = null, NodeProjection secondNode = null)
            : this(original.CommandPriorityData, original.Id, original.CurrentHp, original.LineType,
                  original, firstNode, secondNode)
        { }   
    }

    public interface IReadOnlyEdgeProjection : INumbered
    {
        public Edge Original { get; }
        public CommandPriorityData CommandPriorityData { get; }
        public NodeProjection FirstNode { get; }
        public NodeProjection SecondNode { get; }
        public int CurrentHp { get; }
        public LineType LineType { get; }
    }
}
