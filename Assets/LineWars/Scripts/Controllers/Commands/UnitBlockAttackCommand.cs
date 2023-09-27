using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace LineWars.Model
{
    public class UnitBlockAttackCommand: UnitAttackCommand
    {
        public UnitBlockAttackCommand(ComponentUnit attacker, IAlive defender) : base(attacker, defender)
        {
        }

        public override string GetLog()
        {
            if (Attacker is MonoBehaviour attackerUnit && Defender is MonoBehaviour blockedUnit)
                return $"{blockedUnit.gameObject.name} перехватил атаку от {attackerUnit.gameObject.name}";
            return $"{Defender.GetType()} перехватил атаку от {Attacker.GetType()}";
        }
    }
}