namespace LineWars.Model
{
    public interface IReadOnlyOwned
    {
        public IReadOnlyBasePlayer Owner { get; }
    }
}