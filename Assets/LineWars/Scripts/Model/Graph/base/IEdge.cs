namespace LineWars.Model
{
    public interface IEdge
    {
        public INode FirsNode { get; }
        public INode SecondNode { get; }

        public INode GetOther(INode node)
        {
            if (FirsNode.Equals(node))
                return SecondNode;
            else
                return FirsNode;
        }
    }
}