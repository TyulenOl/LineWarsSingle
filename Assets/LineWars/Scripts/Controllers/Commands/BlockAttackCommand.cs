using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace LineWars.Model
{
    public class BlockAttackCommand: UnitAttackCommand
    {
        public BlockAttackCommand(ComponentUnit attacker, IAlive defender) : base(attacker, defender)
        {
        }

        public override string GetLog()
        {
            if (attacker is MonoBehaviour attackerUnit && defender is MonoBehaviour blockedUnit)
                return $"{blockedUnit.gameObject.name} перехватил атаку от {attackerUnit.gameObject.name}";
            return $"{defender.GetType()} перехватил атаку от {attacker.GetType()}";
        }
    }
}