namespace LineWars.Model
{
    public interface INodeVisitor<out T>
    {
        public T Visit(Node node);
        public T Visit(NodeProjection projection);
    }
}