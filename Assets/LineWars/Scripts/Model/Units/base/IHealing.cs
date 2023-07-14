namespace LineWars.Model
{
    public interface IHealing
    {
        public bool IsMassHeal { get; }
        public int HealingAmount { get; }
    }
}