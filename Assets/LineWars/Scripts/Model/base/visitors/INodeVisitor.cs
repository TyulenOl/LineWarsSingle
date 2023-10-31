namespace LineWars.Model
{
    public interface INodeVisitor<T>
    {
        public T Visit(Node node);
        public T Visit(NodeProjection projection);
    }
}