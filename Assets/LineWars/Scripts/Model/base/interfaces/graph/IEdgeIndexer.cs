namespace LineWars.Model
{
    public interface IEdgeIndexer<TNode, TEdge>
        where TNode : class, INode<TNode, TEdge>
        where TEdge : class, IEdge<TNode, TEdge>
    {
        public TEdge this[int id] { get; set; }
    }
}