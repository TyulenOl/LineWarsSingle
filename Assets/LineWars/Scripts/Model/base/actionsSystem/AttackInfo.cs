namespace LineWars.Model
{
    public struct AttackInfo
    {
        public readonly AttackType AttackType;
        public readonly int Amount;

        public AttackInfo(AttackType attackType, int amount)
        {
            AttackType = attackType;
            Amount = amount;
        }
    }
}