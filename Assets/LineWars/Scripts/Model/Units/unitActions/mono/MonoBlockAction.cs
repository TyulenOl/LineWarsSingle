using System;
using UnityEngine;

namespace LineWars.Model
{
    [RequireComponent(typeof(MonoAttackAction))]
    public class MonoBlockAction : MonoUnitAction, 
        IBlockAction<Node, Edge, Unit, Owned, BasePlayer, Nation>
    {
        private BlockAction<Node, Edge, Unit, Owned, BasePlayer, Nation> BlockAction
            => (BlockAction<Node, Edge, Unit, Owned, BasePlayer, Nation>) ExecutorAction;
        
        [SerializeField] private bool initialProtection = false;
        [SerializeField] private IntModifier initialContrAttackDamageModifier;
        public IntModifier InitialContrAttackDamageModifier => initialContrAttackDamageModifier;
        public bool InitialProtection => initialProtection;
        public event Action<bool, bool> CanBlockChanged;
        public bool IsBlocked => BlockAction.IsBlocked;
        public bool CanBlock() => BlockAction.CanBlock();
        public void EnableBlock() => BlockAction.EnableBlock();

        public bool CanContrAttack(Unit enemy) => BlockAction.CanContrAttack(enemy);

        public void ContrAttack(Unit enemy) => BlockAction.CanContrAttack(enemy);

        protected override ExecutorAction GetAction()
        {
            var action = new BlockAction<Node, Edge, Unit, Owned, BasePlayer, Nation>(GetComponent<Unit>(), this);
            action.CanBlockChanged += (before, after) => CanBlockChanged?.Invoke(before, after); 
            return action;
        }
    }
}