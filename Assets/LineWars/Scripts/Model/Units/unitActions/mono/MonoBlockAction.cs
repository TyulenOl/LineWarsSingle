using JetBrains.Annotations;
using System;
using UnityEngine;

namespace LineWars.Model
{
    [RequireComponent(typeof(MonoAttackAction))]
    public class MonoBlockAction :
        MonoUnitAction<BlockAction<Node, Edge, Unit, Owned, BasePlayer>>,
        IBlockAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        private BlockAction<Node, Edge, Unit, Owned, BasePlayer> BlockAction
            => (BlockAction<Node, Edge, Unit, Owned, BasePlayer>) Action;

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

        protected override BlockAction<Node, Edge, Unit, Owned, BasePlayer> GetAction()
        {
            var action = new BlockAction<Node, Edge, Unit, Owned, BasePlayer>(Unit,
                InitialContrAttackDamageModifier,
                InitialProtection);
            action.CanBlockChanged += (before, after) => CanBlockChanged?.Invoke(before, after);
            return action;
        }

        public ICommandWithCommandType GenerateCommand()
        {
            return new BlockCommand<Node, Edge, Unit, Owned, BasePlayer>(this);
        }

        public override void Accept(IMonoUnitVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}