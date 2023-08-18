namespace LineWars.Model
{
    public interface IMovable
    {
        public Node Node { get; }
        public void MoveTo(Node target);
        public bool CanMoveTo(Node target);
    }
}