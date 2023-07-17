namespace LineWars.Model
{
    public interface IAlive
    {
        public int Hp { get; }

        public void DealDamage(Hit hit);
    }
}