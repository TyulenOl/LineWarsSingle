using UnityEngine;

namespace LineWars.Model
{
    public class MeleeAttackAIAction : AIAction
    {
        [SerializeField] private PointerToUnit myUnit;
        [SerializeField] private PointerToUnit enemyUnit;

        public override void Execute()
        {
            myUnit.GetUnit().GetAction<MonoMeleeAttackAction>().Attack(enemyUnit.GetUnit());
        }
    }
}