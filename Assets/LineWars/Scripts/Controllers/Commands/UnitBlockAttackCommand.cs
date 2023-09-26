using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace LineWars.Model
{
    public class BlockAttackCommand: UnitAttackCommand
    {
        public BlockAttackCommand(ModelComponentUnit attacker, IAlive defender) : base(attacker, defender)
        {
        }

        public override string GetLog()
        {
            return $"{Defender} перехватил атаку от {Attacker}";
        }
    }
}