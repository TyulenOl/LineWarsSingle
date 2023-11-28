using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class MeleeAttackAITurn : AITurn
    {
        [SerializeField] private PointerToUnit myUnit;
        [SerializeField] private PointerToUnit enemyUnit;

        public override void Execute()
        {
            myUnit.GetUnit().GetAction<MonoMeleeAttackAction>().Attack(enemyUnit.GetUnit());
        }
    }
}