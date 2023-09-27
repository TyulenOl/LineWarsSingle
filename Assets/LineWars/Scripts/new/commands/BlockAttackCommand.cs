namespace LineWars.Model
{
    public class BlockAttackCommand: AttackCommand
    {
        public BlockAttackCommand(IUnit attacker, IAlive defender) : base(attacker, defender)
        {}

        public BlockAttackCommand(AttackAction attackAction, IAlive alive) : base(attackAction, alive)
        {}
        
        public override string GetLog()
        {
            return $"{Defender} перехватил атаку от {Attacker}";
        }
    }
}