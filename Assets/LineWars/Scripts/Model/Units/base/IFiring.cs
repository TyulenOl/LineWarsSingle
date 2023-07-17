namespace LineWars.Model
{
    public interface IFiring: IAttackerVisitor
    {
        public bool IsExplosive { get; }
        public int FireDistance { get; }
        public int FireDamage { get; }
    }
}