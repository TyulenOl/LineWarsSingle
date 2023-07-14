namespace LineWars.Model
{
    public interface IFiring
    {
        public bool IsExplosive { get; }
        public int FireDistance { get; }
        public int FireDamage { get; }
    }
}