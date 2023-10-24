using JetBrains.Annotations;
using System;
using UnityEngine;

namespace LineWars.Model
{
    [RequireComponent(typeof(MonoAttackAction))]
    public class MonoBlockAction : MonoUnitAction, 
        IBlockAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        private BlockAction<Node, Edge, Unit, Owned, BasePlayer> BlockAction
            => (BlockAction<Node, Edge, Unit, Owned, BasePlayer>) ExecutorAction;
        
        [SerializeField] private bool initialProtection = false;
        [SerializeField] private IntModifier initialContrAttackDamageModifier;
        public IntModifier InitialContrAttackDamageModifier => initialContrAttackDamageModifier;
        public bool InitialProtection => initialProtection;
        
        public bool Protection => BlockAction.Protection;
        public bool IsBlocked => BlockAction.IsBlocked;
        public event Action<bool, bool> CanBlockChanged;
        
        public bool CanBlock() => BlockAction.CanBlock();
        public void EnableBlock() => BlockAction.EnableBlock();

        public bool CanContrAttack(Unit enemy) => BlockAction.CanContrAttack(enemy);
        public void ContrAttack(Unit enemy) => BlockAction.CanContrAttack(enemy);

        protected override ExecutorAction GetAction()
        {
            var action = new BlockAction<Node, Edge, Unit, Owned, BasePlayer>(GetComponent<Unit>(), this);
            action.CanBlockChanged += (before, after) => CanBlockChanged?.Invoke(before, after); 
            return action;
        }

        public ICommand GenerateCommand()
        {
            return new BlockCommand<Node, Edge, Unit, Owned, BasePlayer>(this);
        }

        public override void Accept(IMonoUnitVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}