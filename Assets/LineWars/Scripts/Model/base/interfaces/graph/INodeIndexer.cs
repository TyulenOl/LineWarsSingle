namespace LineWars.Model
{
    public interface INodeIndexer<TNode, TEdge>
        where TNode : class, INode<TNode, TEdge>
        where TEdge : class, IEdge<TNode, TEdge>
    {
        public TNode this[int id] { get; set; }
    }
}