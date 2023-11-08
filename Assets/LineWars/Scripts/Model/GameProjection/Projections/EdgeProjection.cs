namespace LineWars.Model
{
    public class EdgeProjection : 
        IEdgeForGame<NodeProjection, EdgeProjection, UnitProjection>,
        IReadOnlyEdgeProjection
    {
        public Edge Original { get; set; }
        public CommandPriorityData CommandPriorityData { get; set; }
        public NodeProjection FirstNode { get; set; }
        public NodeProjection SecondNode { get; set; }
        public int Id { get; set; }
        public int MaxHp { get; set; }
        public int CurrentHp { get; set; }
        public LineType LineType { get; set; }

        public bool HasOriginal => Original != null;

        public EdgeProjection()
        {
        }
    }

    public interface IReadOnlyEdgeProjection : INumbered
    {
        public Edge Original { get; }
        public CommandPriorityData CommandPriorityData { get; }
        public NodeProjection FirstNode { get; }
        public NodeProjection SecondNode { get; }
        public int CurrentHp { get; }
        public int MaxHp { get; }
        public LineType LineType { get; }
    }
}
